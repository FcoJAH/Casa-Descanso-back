using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaDescanso.Domain.Entities;

public class Worker
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Required, MaxLength(100)]
    public string MiddleName { get; set; } = null!;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required, MaxLength(20)]
    public string Gender { get; set; } = null!;

    [MaxLength(255)]
    public string? PhotoPath { get; set; }

    [Required, MaxLength(20)]
    public string Phone { get; set; } = null!;

    [MaxLength(150)]
    public string? Email { get; set; }

    [Required, MaxLength(150)]
    public string EmergencyContactName { get; set; } = null!;

    [Required, MaxLength(20)]
    public string EmergencyContactPhone { get; set; } = null!;

    [MaxLength(13)]
    public string? RFC { get; set; }

    [MaxLength(18)]
    public string? CURP { get; set; }

    [MaxLength(20)]
    public string? NSS { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    [Required, MaxLength(100)]
    public string EducationLevel { get; set; } = null!;

    public string? Allergies { get; set; }

    public int ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public UserAccount UserAccount { get; set; } = null!;
}
