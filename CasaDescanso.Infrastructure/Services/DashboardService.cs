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
        var timezoneId = OperatingSystem.IsWindows()
        ? "Central Standard Time (Mexico)"
        : "America/Mexico_City";

        var gdlZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        var nowGdl = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, gdlZone);
        var hoy = nowGdl.Date;
        var tomorrow = hoy.AddDays(1);

        //Obtener lista de nombres de trabajadores activos (OPEN)
        var activeWorkersList = await _context.Attendances
            .Include(a => a.User)
                .ThenInclude(u => u.Worker)
            .Where(a => a.Status == "OPEN")
            .Select(a => a.User.Worker.FirstName + " " + a.User.Worker.LastName + " " + a.User.Worker.MiddleName)
            .ToListAsync();

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
                .CountAsync(a => a.Date == hoy),

            //Lista de nombres para el Front
            ActiveWorkersNames = activeWorkersList
        };
    }
}
