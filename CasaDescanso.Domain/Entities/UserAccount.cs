using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaDescanso.Domain.Entities;

[Table("useraccounts")]
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

    public bool HasSeenSupportAnnouncement { get; set; } = false;

    public DateTime CreatedAt { get; set; }

    public UserAccount()
    {
        var timezoneId = OperatingSystem.IsWindows()
            ? "Central Standard Time (Mexico)"
            : "America/Mexico_City";

        var gdlZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, gdlZone);
    }

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Incident> RegisteredIncidents { get; set; } = new List<Incident>();
    public ICollection<VitalSign> VitalSigns { get; set; } = new List<VitalSign>();
}

