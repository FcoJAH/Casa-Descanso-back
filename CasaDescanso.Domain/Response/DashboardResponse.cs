namespace CasaDescanso.Domain.Response;

public class DashboardResponse
{
    public int TotalResidents { get; set; }
    public int ActiveResidents { get; set; }
    public int InactiveResidents { get; set; }

    public int TotalWorkers { get; set; }
    public int ActiveWorkers { get; set; }
    public int InactiveWorkers { get; set; }

    public int TotalIncidents { get; set; }
    public int TodayIncidents { get; set; }

    public int WorkersWorkingNow { get; set; }
    public int CheckInsToday { get; set; }
}
