namespace CasaDescanso.Domain.Responses.WorkerDetailResponse;

public class WorkerDetailResponse
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = null!;
    public string? PhotoPath { get; set; }
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }
    public string EmergencyContactName { get; set; } = null!;
    public string EmergencyContactPhone { get; set; } = null!;
    public string? RFC { get; set; }
    public string? CURP { get; set; }
    public string? NSS { get; set; }
    public int RoleId { get; set; }
    public string EducationLevel { get; set; } = null!;
    public string? Allergies { get; set; }
    public int ShiftId { get; set; }
    public int Id { get; set; }
}

