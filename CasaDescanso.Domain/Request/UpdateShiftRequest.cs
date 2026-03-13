namespace CasaDescanso.Domain.Requests.UpdateShiftRequest;
public class UpdateShiftRequest
{
    public string Name { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
