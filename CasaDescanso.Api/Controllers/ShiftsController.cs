using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CasaDescanso.Infrastructure.Data;
using CasaDescanso.Domain.Entities;
using CasaDescanso.Domain.Requests.CreateShiftRequest;
using CasaDescanso.Domain.Requests.UpdateShiftRequest;

[Route("api/[controller]")]
[ApiController]
public class ShiftsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ShiftsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateShift([FromBody] CreateShiftRequest request)
    {
        var shift = new Shift
        {
            Name = request.Name,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
        };

        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Horario registrado correctamente",
            shiftId = shift.Id,
            shift.Name,
            shift.StartTime,
            shift.EndTime
        });
    }

    // PUT: api/shifts/{id}
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateShift(int id, [FromBody] UpdateShiftRequest request)
    {
        var shift = await _context.Shifts.FindAsync(id);

        if (shift == null)
            return NotFound("Horario no encontrado.");

        if (request.StartTime >= request.EndTime)
            return BadRequest("La hora de inicio debe ser menor que la hora de fin.");

        // Validar nombre duplicado (excepto el mismo registro)
        var exists = await _context.Shifts
            .AnyAsync(s => s.Name == request.Name && s.Id != id);

        if (exists)
            return BadRequest("Ya existe otro turno con ese nombre.");

        // Actualización
        shift.Name = request.Name;
        shift.StartTime = request.StartTime;
        shift.EndTime = request.EndTime;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Horario actualizado correctamente",
            shiftId = shift.Id,
            shift.Name,
            shift.StartTime,
            shift.EndTime
        });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        var shift = await _context.Shifts
            .Include(s => s.Workers)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shift == null)
            return NotFound("Horario no encontrado.");

        if (shift.Workers.Any())
            return BadRequest("No se puede eliminar el horario porque tiene trabajadores asignados.");

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Horario eliminado correctamente",
            shiftId = id
        });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetShifts()
    {
        var shifts = await _context.Shifts
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.StartTime,
                s.EndTime
            })
            .ToListAsync();

        return Ok(shifts);
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetShiftById(int id)
    {
        var shift = await _context.Shifts
            .Include(s => s.Workers)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shift == null)
            return NotFound("Horario no encontrado.");

        return Ok(new
        {
            shift.Id,
            shift.Name,
            shift.StartTime,
            shift.EndTime,
            WorkersCount = shift.Workers.Count
        });
    }
}
