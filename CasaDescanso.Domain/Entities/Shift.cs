using System.ComponentModel.DataAnnotations;

namespace CasaDescanso.Domain.Entities;

public class Shift
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
