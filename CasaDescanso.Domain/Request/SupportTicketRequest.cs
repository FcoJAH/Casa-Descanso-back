namespace CasaDescanso.Domain.Request;

public class SupportTicketRequest
{
    public string Description { get; set; } = string.Empty;
    public string CurrentUrl { get; set; } = string.Empty;
    public string ReporterName { get; set; } = string.Empty;
    public string ReporterRole { get; set; } = string.Empty;
    public string LocalTime { get; set; } = string.Empty;
    public string? ScreenshotBase64 { get; set; }
}
