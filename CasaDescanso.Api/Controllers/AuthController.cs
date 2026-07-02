using CasaDescanso.Api.DTOs.Auth;
using CasaDescanso.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CasaDescanso.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request.Username, request.Password);

        if (!result.IsSuccess)
            return Unauthorized("Usuario o contraseña incorrectos");

        var response = new LoginResponseDto
        {
            UserId = result.UserId,
            WorkerId = result.WorkerId,
            FullName = result.FullName,
            Position = result.Position,
            Shift = result.ShiftName,
            HasSeenSupportAnnouncement = result.HasSeenSupportAnnouncement,
            Token = result.Token,
            RefreshToken = result.RefreshToken
        };

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CasaDescanso.Api.DTOs.Auth.RefreshRequestDto request)
    {
        var result = await _authService.RefreshAsync(request.Token, request.RefreshToken);

        if (!result.IsSuccess)
            return Unauthorized("Sesión expirada o token inválido.");

        var response = new LoginResponseDto
        {
            UserId = result.UserId,
            WorkerId = result.WorkerId,
            FullName = result.FullName,
            Position = result.Position,
            Shift = result.ShiftName,
            HasSeenSupportAnnouncement = result.HasSeenSupportAnnouncement,
            Token = result.Token,
            RefreshToken = result.RefreshToken
        };

        return Ok(response);
    }

    [HttpPost("{userId}/mark-support-announcement")]
    public async Task<IActionResult> MarkSupportAnnouncementAsSeen(int userId)
    {
        var success = await _authService.MarkAnnouncementAsSeenAsync(userId);
        if (!success) return NotFound("Usuario no encontrado.");

        return Ok(new { message = "Anuncio marcado como visto." });
    }
}
