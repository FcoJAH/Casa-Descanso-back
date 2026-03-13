namespace CasaDescanso.Domain.Entities;

public class ResidentDiet
{
    public int Id { get; set; }

    public int ResidentId { get; set; }
    public Resident Resident { get; set; } = null!;

    public int DietTypeId { get; set; }
    public DietType DietType { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
