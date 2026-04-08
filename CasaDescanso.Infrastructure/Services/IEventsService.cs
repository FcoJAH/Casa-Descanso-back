using CasaDescanso.Domain.Entities;

namespace CasaDescanso.Application.Interfaces; // Ajusta el namespace a tu carpeta de interfaces

public interface IEventsService
{
    Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    Task<Event> CreateEventAsync(Event newEvent);
    Task<bool> UpdateEventAsync(int id, Event updatedEvent);
    Task<bool> DeleteEventAsync(int id);
}