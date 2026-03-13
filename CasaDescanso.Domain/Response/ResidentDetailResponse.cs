namespace CasaDescanso.Domain.Responses;

public class ResidentDetailResponse
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;

    public string NSS { get; set; } = string.Empty;
    public string? PhotoPath { get; set; }

    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;
    public string EmergencyContactRelation { get; set; } = string.Empty;

    public string? SecondContactName { get; set; }
    public string? SecondContactPhone { get; set; }

    public string? DiagnosedDiseases { get; set; }
    public string? Allergies { get; set; }

    public string BloodType { get; set; } = string.Empty;

    public DateTime AdmissionDate { get; set; }

    public string? Observations { get; set; }

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
