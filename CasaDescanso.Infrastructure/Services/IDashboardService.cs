using CasaDescanso.Domain.Response;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardAsync();
}
