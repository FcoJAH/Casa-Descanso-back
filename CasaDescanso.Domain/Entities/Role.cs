using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaDescanso.Domain.Entities;

[Table("roles")]
public class Role
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    [MaxLength(150)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();

    public Role()
    {
        var timezoneId = OperatingSystem.IsWindows()
            ? "Central Standard Time (Mexico)"
            : "America/Mexico_City";

        var gdlZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, gdlZone);
    }
}
