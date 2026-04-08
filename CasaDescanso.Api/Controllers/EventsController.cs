using CasaDescanso.Application.Interfaces;
using CasaDescanso.Domain.Entities;
using CasaDescanso.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    // Cambiamos el contexto por el servicio para respetar la arquitectura de capas
    private readonly IEventsService _eventsService;

    public EventsController(IEventsService eventsService)
    {
        // Corregido: Asignación correcta del parámetro
        _eventsService = eventsService;
    }

    // GET: api/Events/getAll
    [HttpGet("getAll")] // Quitamos el "/" inicial para que sea relativo a api/Events
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        // Delegamos la lógica al servicio
        var events = await _eventsService.GetUpcomingEventsAsync();

        // El filtrado de fecha ya lo haces en el TS, pero si el servicio ya lo trae filtrado, mejor.
        return Ok(events);
    }

    // POST: api/Events/create
    [HttpPost("create")]
    public async Task<ActionResult<Event>> PostEvent(Event newEvent)
    {
        if (newEvent == null) return BadRequest();

        // Forzamos mayúsculas (Regla del sistema)
        newEvent.Title = newEvent.Title.ToUpper();

        var createdEvent = await _eventsService.CreateEventAsync(newEvent);

        return CreatedAtAction(nameof(GetEvents), new { id = createdEvent.Id }, createdEvent);
    }

    // DELETE: api/Events/5/delete
    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var result = await _eventsService.DeleteEventAsync(id);
        if (!result) return NotFound();

        return NoContent();
    }
}