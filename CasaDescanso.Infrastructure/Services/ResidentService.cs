using CasaDescanso.Domain.Entities;
using CasaDescanso.Domain.Request;
using CasaDescanso.Domain.Request.UpdateResidentRequest;
using CasaDescanso.Domain.Response;
using CasaDescanso.Domain.Responses;
using CasaDescanso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ResidentService : IResidentService
{
    private readonly ApplicationDbContext _context;

    public ResidentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResidentResponse> CreateAsync(CreateResidentRequest request)
    {
        // Validar NSS único
        var exists = await _context.Residents
            .AnyAsync(r => r.NSS == request.NSS);

        if (exists)
            throw new Exception("Ya existe un residente con ese NSS.");

        var resident = new Resident
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            NSS = request.NSS,
            PhotoPath = request.PhotoPath,
            EmergencyContactName = request.EmergencyContactName,
            EmergencyContactPhone = request.EmergencyContactPhone,
            EmergencyContactRelation = request.EmergencyContactRelation,
            SecondContactName = request.SecondContactName,
            SecondContactPhone = request.SecondContactPhone,
            DiagnosedDiseases = request.DiagnosedDiseases,
            Allergies = request.Allergies,
            BloodType = request.BloodType,
            AdmissionDate = request.AdmissionDate,
            Observations = request.Observations
        };

        _context.Residents.Add(resident);
        await _context.SaveChangesAsync();

        return new ResidentResponse
        {
            Id = resident.Id,
            FullName = $"{resident.FirstName} {resident.LastName} {resident.MiddleName}",
            NSS = resident.NSS,
            AdmissionDate = resident.AdmissionDate,
            IsActive = resident.IsActive
        };
    }

    public async Task<ResidentDetailResponse?> GetByIdAsync(int id)
    {
        var resident = await _context.Residents
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resident == null)
            return null;

        return new ResidentDetailResponse
        {
            Id = resident.Id,
            FirstName = resident.FirstName,
            LastName = resident.LastName,
            MiddleName = resident.MiddleName,
            BirthDate = resident.BirthDate,
            Gender = resident.Gender,
            NSS = resident.NSS,
            PhotoPath = resident.PhotoPath,
            EmergencyContactName = resident.EmergencyContactName,
            EmergencyContactPhone = resident.EmergencyContactPhone,
            EmergencyContactRelation = resident.EmergencyContactRelation,
            SecondContactName = resident.SecondContactName,
            SecondContactPhone = resident.SecondContactPhone,
            DiagnosedDiseases = resident.DiagnosedDiseases,
            Allergies = resident.Allergies,
            BloodType = resident.BloodType,
            AdmissionDate = resident.AdmissionDate,
            Observations = resident.Observations,
            IsActive = resident.IsActive,
            CreatedAt = resident.CreatedAt
        };
    }

    public async Task<Resident?> UpdateResidentAsync(int id, UpdateResidentRequest dto)
    {
        var resident = await _context.Residents
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resident == null)
            return null;

        resident.FirstName = dto.FirstName;
        resident.LastName = dto.LastName;
        resident.MiddleName = dto.MiddleName;
        resident.BirthDate = dto.BirthDate;
        resident.Gender = dto.Gender;
        resident.NSS = dto.NSS;
        resident.PhotoPath = dto.PhotoPath;
        resident.EmergencyContactName = dto.EmergencyContactName;
        resident.EmergencyContactPhone = dto.EmergencyContactPhone;
        resident.EmergencyContactRelation = dto.EmergencyContactRelation;
        resident.SecondContactName = dto.SecondContactName;
        resident.SecondContactPhone = dto.SecondContactPhone;
        resident.DiagnosedDiseases = dto.DiagnosedDiseases;
        resident.Allergies = dto.Allergies;
        resident.BloodType = dto.BloodType;
        resident.AdmissionDate = dto.AdmissionDate;
        resident.Observations = dto.Observations;

        await _context.SaveChangesAsync();

        return resident;
    }

    public async Task<bool> ToggleActiveAsync(int id)
    {
        var resident = await _context.Residents
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resident == null)
            return false;

        resident.IsActive = !resident.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Resident>> GetAllAsync()
    {
        return await _context.Residents
            .OrderBy(r => r.LastName)
            .ToListAsync();
    }

    public async Task<List<Resident>> GetActiveAsync()
    {
        return await _context.Residents
            .Where(r => r.IsActive)
            .OrderBy(r => r.LastName)
            .ToListAsync();
    }

    public async Task<List<Resident>> GetInactiveAsync()
    {
        return await _context.Residents
            .Where(r => !r.IsActive)
            .OrderBy(r => r.LastName)
            .ToListAsync();
    }
}
