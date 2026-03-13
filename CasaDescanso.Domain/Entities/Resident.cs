using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaDescanso.Domain.Entities;

[Table("Residents")]
public class Resident
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

    [Required, MaxLength(20)]
    public string NSS { get; set; } = null!;

    [MaxLength(255)]
    public string? PhotoPath { get; set; }

    [Required, MaxLength(150)]
    public string EmergencyContactName { get; set; } = null!;

    [Required, MaxLength(20)]
    public string EmergencyContactPhone { get; set; } = null!;

    [Required, MaxLength(100)]
    public string EmergencyContactRelation { get; set; } = null!;

    [MaxLength(150)]
    public string? SecondContactName { get; set; }

    [MaxLength(20)]
    public string? SecondContactPhone { get; set; }

    public string? DiagnosedDiseases { get; set; }

    public string? Allergies { get; set; }

    [Required, MaxLength(5)]
    public string BloodType { get; set; } = null!;

    [Required]
    public DateTime AdmissionDate { get; set; }

    public string? Observations { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
