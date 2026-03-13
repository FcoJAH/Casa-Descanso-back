namespace CasaDescanso.Domain.Request;
public class UpdateIncidentRequest
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = null!;
    public string SeverityLevel { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Resolved { get; set; }
}
