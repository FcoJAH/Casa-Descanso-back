namespace CasaDescanso.Domain.Response;

public class VitalSignResponse
{
    public int Id { get; set; }
    public decimal? Temperature { get; set; }
    public string? BloodPressure { get; set; }
    public int? HeartRate { get; set; }
    public int? RespiratoryFrequency { get; set; }
    public decimal? OxygenSaturation { get; set; }
    public decimal? GlucoseLevel { get; set; }
    public decimal? Weight { get; set; }
    public string? Notes { get; set; }
    public DateTime RecordedAt { get; set; }
    public string RecordedBy { get; set; } = string.Empty;
}
