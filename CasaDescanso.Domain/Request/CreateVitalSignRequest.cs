namespace CasaDescanso.Domain.Request;

public class CreateVitalSignRequest
{
    public int ResidentId { get; set; }
    public int RecordedByUserId { get; set; }

    public decimal? Temperature { get; set; }
    public string? BloodPressure { get; set; }
    public int? HeartRate { get; set; }
    public int? RespiratoryFrequency { get; set; }
    public decimal? OxygenSaturation { get; set; }
    public decimal? GlucoseLevel { get; set; }
    public decimal? Weight { get; set; }

    public string? Notes { get; set; }
}
