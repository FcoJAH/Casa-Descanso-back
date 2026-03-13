using System.ComponentModel.DataAnnotations;

namespace CasaDescanso.Domain.Entities;

public class Role
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    [MaxLength(150)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
}
