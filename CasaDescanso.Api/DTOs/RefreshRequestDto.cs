namespace CasaDescanso.Api.DTOs.Auth;

public class RefreshRequestDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
