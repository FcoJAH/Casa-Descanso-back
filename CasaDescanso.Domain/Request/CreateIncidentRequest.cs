namespace CasaDescanso.Domain.Request;
public class CreateIncidentRequest
{
    public int ResidentId { get; set; }
    public int RegisteredByUserId { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = null!;
    public string SeverityLevel { get; set; } = null!;
    public string Description { get; set; } = null!;
}
