using CasaDescanso.Domain.Entities;
using CasaDescanso.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Events
    [HttpGet("/getAll")]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        // Traemos los eventos de hoy en adelante, ordenados por fecha próxima
        return await _context.Events
            .Where(e => e.EventDate >= DateTime.Today)
            .OrderBy(e => e.EventDate)
            .Take(10) // El cliente quiere ver qué viene, subimos a 10
            .ToListAsync();
    }

    // POST: api/Events
    [HttpPost("/create")]
    public async Task<ActionResult<Event>> PostEvent(Event newEvent)
    {
        // Forzamos mayúsculas en el título como en el resto de tu sistema
        newEvent.Title = newEvent.Title.ToUpper();

        // No enviamos CreatedAt porque tu SQL tiene el DEFAULT
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvents), new { id = newEvent.Id }, newEvent);
    }

    // PUT: api/Events/5
    [HttpPut("{id}/modify")]
    public async Task<IActionResult> PutEvent(int id, Event updatedEvent)
    {
        if (id != updatedEvent.Id) return BadRequest();

        var eventInDb = await _context.Events.FindAsync(id);
        if (eventInDb == null) return NotFound();

        eventInDb.Title = updatedEvent.Title.ToUpper();
        eventInDb.Description = updatedEvent.Description;
        eventInDb.EventDate = updatedEvent.EventDate;
        // createdBy y createdAt no se deberían editar

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Events/5
    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var eventToDelete = await _context.Events.FindAsync(id);
        if (eventToDelete == null) return NotFound();

        _context.Events.Remove(eventToDelete);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}