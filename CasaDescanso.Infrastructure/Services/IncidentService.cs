using CasaDescanso.Domain.Entities;
using CasaDescanso.Domain.Request;
using CasaDescanso.Domain.Response;
using CasaDescanso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class IncidentService : IIncidentService
{
    private readonly ApplicationDbContext _context;

    public IncidentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(CreateIncidentRequest request)
    {
        // Validar que exista el residente
        var residentExists = await _context.Residents
            .AnyAsync(r => r.Id == request.ResidentId);

        if (!residentExists)
            throw new Exception("Residente no encontrado");

        // Validar que exista el usuario que registra
        var userExists = await _context.UserAccounts
            .AnyAsync(u => u.Id == request.RegisteredByUserId);

        if (!userExists)
            throw new Exception("Usuario no encontrado");

        var incident = new Incident
        {
            ResidentId = request.ResidentId,
            RegisteredByUserId = request.RegisteredByUserId,
            Date = request.Date,
            Type = request.Type,
            SeverityLevel = request.SeverityLevel,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        _context.Incidents.Add(incident);
        await _context.SaveChangesAsync();

        return incident.Id;
    }

    public async Task<IncidentDetailResponse?> GetByIdAsync(int id)
    {
        return await _context.Incidents
            .Include(i => i.Resident)
            .Include(i => i.RegisteredByUser)
            .Where(i => i.Id == id)
            .Select(i => new IncidentDetailResponse
            {
                Id = i.Id,
                ResidentId = i.ResidentId,
                ResidentFullName = i.Resident.FirstName + " " + i.Resident.LastName,
                RegisteredByUserId = i.RegisteredByUserId,
                RegisteredByUsername = i.RegisteredByUser.Username,
                Date = i.Date,
                Type = i.Type,
                SeverityLevel = i.SeverityLevel,
                Description = i.Description,
                CreatedAt = i.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(int id, UpdateIncidentRequest request)
    {
        var incident = await _context.Incidents.FindAsync(id);

        if (incident == null)
            return false;

        incident.Date = request.Date;
        incident.Type = request.Type;
        incident.SeverityLevel = request.SeverityLevel;
        incident.Description = request.Description;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<IncidentDetailResponse>> GetByResidentIdAsync(int residentId)
    {
        return await _context.Incidents
            .Include(i => i.Resident)
            .Include(i => i.RegisteredByUser)
            .Where(i => i.ResidentId == residentId)
            .OrderByDescending(i => i.Date)
            .Select(i => new IncidentDetailResponse
            {
                Id = i.Id,
                ResidentId = i.ResidentId,
                ResidentFullName = i.Resident.FirstName + " " + i.Resident.LastName,
                RegisteredByUserId = i.RegisteredByUserId,
                RegisteredByUsername = i.RegisteredByUser.Username,
                Date = i.Date,
                Type = i.Type,
                SeverityLevel = i.SeverityLevel,
                Description = i.Description,

                CreatedAt = i.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<IncidentDetailResponse>> GetAllAsync()
    {
        return await _context.Incidents
            .Include(i => i.Resident)
            .Include(i => i.RegisteredByUser)
            .OrderByDescending(i => i.Date)
            .Select(i => new IncidentDetailResponse
            {
                Id = i.Id,
                ResidentId = i.ResidentId,
                ResidentFullName = i.Resident.FirstName + " " + i.Resident.LastName,
                RegisteredByUserId = i.RegisteredByUserId,
                RegisteredByUsername = i.RegisteredByUser.Username,
                Date = i.Date,
                Type = i.Type,
                SeverityLevel = i.SeverityLevel,
                Description = i.Description,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync();
    }
}
