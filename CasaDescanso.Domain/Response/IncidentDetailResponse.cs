namespace CasaDescanso.Domain.Response;
public class IncidentDetailResponse
{
    public int Id { get; set; }
    public int ResidentId { get; set; }
    public string ResidentFullName { get; set; } = null!;
    public int RegisteredByUserId { get; set; }
    public string RegisteredByUsername { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Type { get; set; } = null!;
    public string SeverityLevel { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Resolved { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
