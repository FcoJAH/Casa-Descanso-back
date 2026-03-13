namespace CasaDescanso.Domain.Entities;

public class VitalSign
{
    public int Id { get; set; }

    public int ResidentId { get; set; }
    public Resident Resident { get; set; } = null!;

    public int RecordedByUserId { get; set; }
    public UserAccount RecordedByUser { get; set; } = null!;

    public decimal? Temperature { get; set; }
    public string? BloodPressure { get; set; }
    public int? HeartRate { get; set; }
    public decimal? OxygenSaturation { get; set; }
    public decimal? GlucoseLevel { get; set; }
    public decimal? Weight { get; set; }

    public string? Notes { get; set; }

    public DateTime RecordedAt { get; set; }
    public int? RespiratoryFrequency { get; set; }

}
