using CasaDescanso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CasaDescanso.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool IsSuccess, int UserId, int WorkerId, string FullName, string Position, string ShiftName, bool HasSeenSupportAnnouncement)>
        LoginAsync(string username, string password)
    {
        var user = await _context.UserAccounts
            .Include(u => u.Worker)
                .ThenInclude(w => w.Role)
            .Include(u => u.Worker)
                .ThenInclude(w => w.Shift)
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

        if (user == null)
            return (false, 0, 0, string.Empty, string.Empty, string.Empty, false);

        // ⚠️ Temporal: comparación directa
        if (user.PasswordHash != password)
            return (false, 0, 0, string.Empty, string.Empty, string.Empty, false);

        var worker = user.Worker;

        var fullName = $"{worker.FirstName} {worker.LastName} {worker.MiddleName}";

        return (
            true,
            user.Id,
            worker.Id,
            fullName,
            worker.Role.Name,
            worker.Shift.Name,
            user.HasSeenSupportAnnouncement
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
}
