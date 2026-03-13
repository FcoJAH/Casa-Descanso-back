using CasaDescanso.Domain.Entities;
using CasaDescanso.Domain.Request;
using CasaDescanso.Domain.Response;
using CasaDescanso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class VitalSignService : IVitalSignService
{
    private readonly ApplicationDbContext _context;

    public VitalSignService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(CreateVitalSignRequest request)
    {
        var residentExists = await _context.Residents
            .AnyAsync(r => r.Id == request.ResidentId);

        if (!residentExists)
            throw new Exception("Residente no encontrado");

        var userExists = await _context.UserAccounts
            .AnyAsync(u => u.Id == request.RecordedByUserId);

        if (!userExists)
            throw new Exception("Usuario no encontrado");

        var vital = new VitalSign
        {
            ResidentId = request.ResidentId,
            RecordedByUserId = request.RecordedByUserId,
            Temperature = request.Temperature,
            BloodPressure = request.BloodPressure,
            HeartRate = request.HeartRate,
            RespiratoryFrequency = request.RespiratoryFrequency,
            OxygenSaturation = request.OxygenSaturation,
            GlucoseLevel = request.GlucoseLevel,
            Weight = request.Weight,
            Notes = request.Notes,
            RecordedAt = DateTime.UtcNow
        };

        _context.VitalSigns.Add(vital);
        await _context.SaveChangesAsync();

        return vital.Id;
    }

    public async Task<List<VitalSignResponse>> GetByResidentAsync(int residentId)
    {
        var residentExists = await _context.Residents
            .AnyAsync(r => r.Id == residentId);

        if (!residentExists)
            throw new Exception("Residente no encontrado");

        return await _context.VitalSigns
            .Where(v => v.ResidentId == residentId)
            .Include(v => v.RecordedByUser)
            .OrderByDescending(v => v.RecordedAt)
            .Select(v => new VitalSignResponse
            {
                Id = v.Id,
                Temperature = v.Temperature,
                BloodPressure = v.BloodPressure,
                HeartRate = v.HeartRate,
                RespiratoryFrequency = v.RespiratoryFrequency,
                OxygenSaturation = v.OxygenSaturation,
                GlucoseLevel = v.GlucoseLevel,
                Weight = v.Weight,
                Notes = v.Notes,
                RecordedAt = v.RecordedAt,
                RecordedBy = v.RecordedByUser.Username
            })
            .ToListAsync();
    }

}
