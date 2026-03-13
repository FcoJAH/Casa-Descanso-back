namespace CasaDescanso.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;

[Table("Attendance")]
public class Attendance
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserAccount User { get; set; } = null!;

    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }

    public DateTime Date { get; set; }

    public string? Notes { get; set; }
    
    public string Status { get; set; } = "OPEN";
}
