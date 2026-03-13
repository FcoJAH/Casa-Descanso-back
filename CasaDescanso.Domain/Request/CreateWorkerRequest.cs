namespace CasaDescanso.Domain.Request.CreateWorkerRequest;

public class CreateWorkerRequest
{
    // Información personal
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;

    // Contacto
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }

    // Contacto de emergencia
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;

    // Información legal (opcionales)
    public string? RFC { get; set; }
    public string? CURP { get; set; }
    public string? NSS { get; set; }

    // Información laboral
    public int RoleId { get; set; }

    public string EducationLevel { get; set; } = string.Empty;
    public string? Allergies { get; set; }

    public int ShiftId { get; set; }

    public string Password { get; set; } = string.Empty;

}
