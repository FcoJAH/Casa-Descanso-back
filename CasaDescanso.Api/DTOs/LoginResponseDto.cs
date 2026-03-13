namespace CasaDescanso.Api.DTOs.Auth;

public class LoginResponseDto
{
    public int UserId { get; set; }
    public int WorkerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Shift { get; set; } = string.Empty;
}
