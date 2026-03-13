using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaDescanso.Domain.Entities;

[Table("Incidents")]
public class Incident
{
    public int Id { get; set; }

    public int ResidentId { get; set; }
    public int RegisteredByUserId { get; set; }

    public DateTime Date { get; set; }

    public string Type { get; set; } = string.Empty;
    public string SeverityLevel { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    // Relaciones
    public Resident Resident { get; set; } = null!;
    public UserAccount RegisteredByUser { get; set; } = null!;
}