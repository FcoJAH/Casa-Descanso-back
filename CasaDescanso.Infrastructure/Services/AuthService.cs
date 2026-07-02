using CasaDescanso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CasaDescanso.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<(bool IsSuccess, int UserId, int WorkerId, string FullName, string Position, string ShiftName, bool HasSeenSupportAnnouncement, string Token, string RefreshToken)>
        LoginAsync(string username, string password)
    {
        var user = await _context.UserAccounts
            .Include(u => u.Worker)
                .ThenInclude(w => w.Role)
            .Include(u => u.Worker)
                .ThenInclude(w => w.Shift)
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

        if (user == null)
            return (false, 0, 0, string.Empty, string.Empty, string.Empty, false, string.Empty, string.Empty);

        // ⚠️ Temporal: comparación directa
        if (user.PasswordHash != password)
            return (false, 0, 0, string.Empty, string.Empty, string.Empty, false, string.Empty, string.Empty);

        var worker = user.Worker;

        var fullName = $"{worker.FirstName} {worker.LastName} {worker.MiddleName}";

        // Generar Tokens
        var token = GenerateJwtToken(user.Id.ToString(), worker.Role.Name);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7 días de validez
        await _context.SaveChangesAsync();

        return (
            true,
            user.Id,
            worker.Id,
            fullName,
            worker.Role.Name,
            worker.Shift.Name,
            user.HasSeenSupportAnnouncement,
            token,
            refreshToken
        );
    }

    public async Task<(bool IsSuccess, int UserId, int WorkerId, string FullName, string Position, string ShiftName, bool HasSeenSupportAnnouncement, string Token, string RefreshToken)>
        RefreshAsync(string token, string refreshToken)
    {
        var user = await _context.UserAccounts
            .Include(u => u.Worker)
                .ThenInclude(w => w.Role)
            .Include(u => u.Worker)
                .ThenInclude(w => w.Shift)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.IsActive);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return (false, 0, 0, string.Empty, string.Empty, string.Empty, false, string.Empty, string.Empty);
        }

        var worker = user.Worker;
        var fullName = $"{worker.FirstName} {worker.LastName} {worker.MiddleName}";

        var newToken = GenerateJwtToken(user.Id.ToString(), worker.Role.Name);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return (
            true,
            user.Id,
            worker.Id,
            fullName,
            worker.Role.Name,
            worker.Shift.Name,
            user.HasSeenSupportAnnouncement,
            newToken,
            newRefreshToken
        );
    }

    public async Task<bool> MarkAnnouncementAsSeenAsync(int userId)
    {
        var user = await _context.UserAccounts.FindAsync(userId);
        if (user == null) return false;

        user.HasSeenSupportAnnouncement = true;
        await _context.SaveChangesAsync();
        return true;
    }

    private string GenerateJwtToken(string userId, string role)
    {
        var jwtKey = _configuration["JwtSettings:Secret"] ?? "EstaEsUnaSuperClaveSecretaMuyLargaParaJWT123456789";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Expiración corta
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
