using System.ComponentModel.DataAnnotations;

namespace CasaDescanso.Domain.Entities;

public class DietType
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<ResidentDiet> ResidentDiets { get; set; } = new List<ResidentDiet>();
}
