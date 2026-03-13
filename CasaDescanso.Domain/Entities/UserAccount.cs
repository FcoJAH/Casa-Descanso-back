using System.ComponentModel.DataAnnotations;

namespace CasaDescanso.Domain.Entities;

public class UserAccount
{
    public int Id { get; set; }

    public int WorkerId { get; set; }
    public Worker Worker { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Username { get; set; } = null!;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Incident> RegisteredIncidents { get; set; } = new List<Incident>();
    public ICollection<VitalSign> VitalSigns { get; set; } = new List<VitalSign>();
}

