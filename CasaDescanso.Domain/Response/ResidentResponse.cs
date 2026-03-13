namespace CasaDescanso.Domain.Response;

public class ResidentResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string NSS { get; set; } = null!;
    public DateTime AdmissionDate { get; set; }
    public bool IsActive { get; set; }
}
