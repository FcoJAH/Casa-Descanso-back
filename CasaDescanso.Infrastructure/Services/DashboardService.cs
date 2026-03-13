using CasaDescanso.Domain.Response;
using CasaDescanso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardResponse> GetDashboardAsync()
    {
        var today = DateTime.UtcNow.Date;
        var hoy = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        return new DashboardResponse
        {
            // Residents
            TotalResidents = await _context.Residents.CountAsync(),
            ActiveResidents = await _context.Residents.CountAsync(r => r.IsActive),
            InactiveResidents = await _context.Residents.CountAsync(r => !r.IsActive),

            // Workers
            TotalWorkers = await _context.Workers.CountAsync(),
            ActiveWorkers = await _context.Workers.CountAsync(w => w.IsActive),
            InactiveWorkers = await _context.Workers.CountAsync(w => !w.IsActive),

            // Incidents
            TotalIncidents = await _context.Incidents.CountAsync(),


            TodayIncidents = await _context.Incidents.Where(i => i.Date >= hoy && i.Date < tomorrow).CountAsync(),

            // Attendance
            WorkersWorkingNow = await _context.Attendances
                .CountAsync(a => a.Status == "OPEN"),

            CheckInsToday = await _context.Attendances
                .CountAsync(a => a.Date == today)
        };
    }
}
