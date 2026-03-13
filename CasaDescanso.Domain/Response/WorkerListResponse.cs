namespace CasaDescanso.Domain.Responses.WorkerListResponse;

public class WorkerListResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
