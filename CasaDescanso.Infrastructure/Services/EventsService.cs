using CasaDescanso.Application.Interfaces;
using CasaDescanso.Domain.Entities;
using CasaDescanso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CasaDescanso.Infrastructure.Services; // Ajusta el namespace a tu carpeta de servicios

public class EventsService : IEventsService
{
    private readonly ApplicationDbContext _context;

    public EventsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
    {
        // Traemos eventos de hoy en adelante, ordenados por fecha
        return await _context.Events
            .Where(e => e.EventDate.Date >= DateTime.Today)
            .OrderBy(e => e.EventDate)
            .Take(10)
            .ToListAsync();
    }

    public async Task<Event> CreateEventAsync(Event newEvent)
    {
        // Regla de negocio: Títulos siempre en Mayúsculas
        newEvent.Title = newEvent.Title.ToUpper();

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return newEvent;
    }

    public async Task<bool> UpdateEventAsync(int id, Event updatedEvent)
    {
        var eventDb = await _context.Events.FindAsync(id);
        if (eventDb == null) return false;

        eventDb.Title = updatedEvent.Title.ToUpper();
        eventDb.Description = updatedEvent.Description;
        eventDb.EventDate = updatedEvent.EventDate;
        // No actualizamos CreatedBy ni CreatedAt para mantener integridad

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var eventDb = await _context.Events.FindAsync(id);
        if (eventDb == null) return false;

        _context.Events.Remove(eventDb);
        await _context.SaveChangesAsync();
        return true;
    }
}