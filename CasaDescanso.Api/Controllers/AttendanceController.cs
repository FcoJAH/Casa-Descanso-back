using CasaDescanso.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CasaDescanso.Domain.Entities;
using CasaDescanso.Domain.Requests.UpdateAttendanceRequest;
using CasaDescanso.Domain.Requests;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AttendanceController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ================================
    // CHECK IN
    // ================================
    [HttpPost("checkin")]
    public async Task<IActionResult> CheckIn([FromBody] AttendanceLocationRequest request)
    {
        var timezoneId = OperatingSystem.IsWindows()
        ? "Central Standard Time (Mexico)"
        : "America/Mexico_City";

        var gdlZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        var nowGdl = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, gdlZone);

        var today = nowGdl.Date; // La fecha "hoy" real en Jalisco

        // Buscamos usando request.UserId
        var user = await _context.UserAccounts
            .FirstOrDefaultAsync(u => u.Id == request.UserId && u.IsActive);

        if (user == null) return BadRequest("Usuario no existente o inactivo.");

        var exists = await _context.Attendances
            .AnyAsync(a => a.UserId == request.UserId && a.Date == today && a.Status == "OPEN");

        if (exists) return BadRequest("Ya registraste ingreso hoy.");

        var attendance = new Attendance
        {
            UserId = request.UserId,
            CheckIn = nowGdl,
            Date = today,
            Status = "OPEN",
            LatitudeIn = request.Latitude,
            LongitudeIn = request.Longitude
        };

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Ingreso registrado correctamente" });
    }


    // ================================
    // CHECK OUT
    // ================================
    [HttpPost("checkout")]
    public async Task<IActionResult> CheckOut([FromBody] AttendanceLocationRequest request)
    {
        var timezoneId = OperatingSystem.IsWindows()
        ? "Central Standard Time (Mexico)"
        : "America/Mexico_City";

        var gdlZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        var nowGdl = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, gdlZone);

        // BUSCAMOS al trabajador que tiene una UserAccount con ese Id
        var worker = await _context.Workers
            .Include(w => w.UserAccount)
            .FirstOrDefaultAsync(w => w.UserAccount.Id == request.UserId && w.IsActive);

        if (worker == null)
            return BadRequest("No se encontró trabajador vinculado a esta cuenta.");

        var userId = worker.UserAccount.Id;

        var attendance = await _context.Attendances
            .FirstOrDefaultAsync(a => a.UserId == userId && a.Status == "OPEN");

        if (attendance == null)
            return BadRequest("No existe registro de ingreso abierto.");

        attendance.CheckOut = nowGdl;
        attendance.Status = "CLOSED";
        attendance.LatitudeOut = request.Latitude;
        attendance.LongitudeOut = request.Longitude;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Salida registrada correctamente" });
    }

    // ================================
    // CONSULTAR HOY
    // ================================
    [HttpGet("today/{workerId}")]
    public async Task<IActionResult> GetToday(int workerId)
    {
        var timezoneId = OperatingSystem.IsWindows()
        ? "Central Standard Time (Mexico)"
        : "America/Mexico_City";

        var gdlZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, gdlZone);

        var attendance = await _context.Attendances
            .Include(a => a.User)
            .ThenInclude(u => u.Worker)
            .Where(a => a.UserId == workerId)
            .Select(a => new
            {
                a.Id,
                a.Date,
                a.CheckIn,
                a.CheckOut,
                User = new
                {
                    a.User.Id,
                    a.User.Username,
                    FullName = a.User.Worker.FirstName + " " +
                            a.User.Worker.LastName + " " +
                            a.User.Worker.MiddleName
                },
                a.Notes
            })
            .FirstOrDefaultAsync();

        if (attendance == null)
            return NotFound("No hay registro hoy.");

        return Ok(attendance);
    }

    // ================================
    // HISTORIAL
    // ================================
    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetHistory(int userId)
    {
        var history = await _context.Attendances
            .Include(a => a.User)
                .ThenInclude(u => u.Worker)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.Date)
            .Select(a => new
            {
                a.Id,
                a.Date,
                a.CheckIn,
                a.CheckOut,
                Worker = a.User.Worker.FirstName + " " +
                         a.User.Worker.LastName + " " +
                         a.User.Worker.MiddleName,
                a.Notes,
                a.Status,
                a.LatitudeIn,
                a.LongitudeIn,
                a.LatitudeOut,
                a.LongitudeOut
            })
            .ToListAsync();

        //if (!history.Any())
        //    return NotFound(new { message = "No hay registros para este usuario" });

        return Ok(history);
    }

    [HttpPut("{id}/adjust")]
    public async Task<IActionResult> AdjustAttendance(int id, UpdateAttendanceRequest request)
    {
        var attendance = await _context.Attendances
            .FirstOrDefaultAsync(a => a.Id == id);

        if (attendance == null)
            return NotFound(new { message = "Registro no encontrado" });

        if (string.IsNullOrWhiteSpace(request.Notes))
            return BadRequest(new { message = "La nota es obligatoria para modificar la asistencia" });

        // Actualizar solo si vienen valores
        if (request.CheckIn.HasValue)
            attendance.CheckIn = request.CheckIn.Value;

        if (request.CheckOut.HasValue)
            attendance.CheckOut = request.CheckOut.Value;

        attendance.Notes = request.Notes;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Asistencia modificada correctamente",
            attendance.Id,
            attendance.CheckIn,
            attendance.CheckOut,
            attendance.Notes,
            attendance.Status
        });
    }

    [HttpGet("status/{userId}")]
    public async Task<IActionResult> HasOpenAttendance(int userId)
    {
        var attendance = await _context.Attendances
            .Where(a => a.UserId == userId && a.Status == "OPEN")
            .Select(a => new
            {
                a.Id,
                a.CheckIn,
                a.Date,
                a.Status
            })
            .FirstOrDefaultAsync();

        if (attendance == null)
        {
            return Ok(new
            {
                hasOpenAttendance = false,
                message = "No existe registro abierto."
            });
        }

        return Ok(new
        {
            hasOpenAttendance = true,
            message = "Existe un registro abierto.",
            attendance
        });
    }

}
