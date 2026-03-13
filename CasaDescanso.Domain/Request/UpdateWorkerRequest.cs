namespace CasaDescanso.Domain.Requests.UpdateWorkerRequest;

public class UpdateWorkerRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;

    public string? PhotoPath { get; set; }

    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }

    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;

    public string? RFC { get; set; }
    public string? CURP { get; set; }
    public string? NSS { get; set; }

    public int RoleId { get; set; }
    public string EducationLevel { get; set; } = string.Empty;

    public string? Allergies { get; set; }

    public int ShiftId { get; set; }
}