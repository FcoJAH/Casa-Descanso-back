namespace CasaDescanso.Domain.Response.CreateWorkerResponse;

public class CreateWorkerResponse
{
    public int WorkerId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
