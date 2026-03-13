namespace CasaDescanso.Domain.Request.UpdateResidentRequest;
public class UpdateResidentRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Gender { get; set; } = null!;
    public string NSS { get; set; } = null!;

    public string? PhotoPath { get; set; }

    public string EmergencyContactName { get; set; } = null!;
    public string EmergencyContactPhone { get; set; } = null!;
    public string EmergencyContactRelation { get; set; } = null!;

    public string? SecondContactName { get; set; }
    public string? SecondContactPhone { get; set; }

    public string? DiagnosedDiseases { get; set; }
    public string? Allergies { get; set; }

    public string BloodType { get; set; } = null!;

    public DateTime AdmissionDate { get; set; }

    public string? Observations { get; set; }
}
