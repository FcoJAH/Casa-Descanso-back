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
            Shift = result.ShiftName
        };

        return Ok(response);
    }
}
