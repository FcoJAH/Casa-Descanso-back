namespace CasaDescanso.Domain.Requests.CreateShiftRequest;
public class CreateShiftRequest
{
    public string Name { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
