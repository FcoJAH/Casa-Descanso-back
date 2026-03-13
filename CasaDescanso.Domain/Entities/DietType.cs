using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaDescanso.Domain.Entities;

[Table("diettypes")]
public class DietType
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<ResidentDiet> ResidentDiets { get; set; } = new List<ResidentDiet>();
}
