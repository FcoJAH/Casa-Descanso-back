using CasaDescanso.Domain.Entities;
using CasaDescanso.Domain.Request.CreateWorkerRequest;
using CasaDescanso.Domain.Requests.UpdateWorkerRequest;
using CasaDescanso.Domain.Response.CreateWorkerResponse;
using CasaDescanso.Domain.Responses.WorkerDetailResponse;
using CasaDescanso.Domain.Responses.WorkerListResponse;
using CasaDescanso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CasaDescanso.Infrastructure.Services;

public class WorkerService : IWorkerService
{
    private readonly ApplicationDbContext _context;

    public WorkerService(ApplicationDbContext context)
    {
        _context = context;
    }

    //**************************************************************//
    // Metodo para crear un nuevo trabajador, se valida que el turno 
    // y el rol existan, luego se crea el trabajador y su cuenta de 
    // usuario asociada
    //**************************************************************//
    public async Task<CreateWorkerResponse> CreateWorkerAsync(CreateWorkerRequest request)
    {
        var shiftExists = await _context.Shifts
            .AnyAsync(s => s.Id == request.ShiftId);

        if (!shiftExists)
            throw new Exception("El turno especificado no existe.");

        var roleExists = await _context.Roles
            .AnyAsync(r => r.Id == request.RoleId && r.IsActive);

        if (!roleExists)
            throw new Exception("El rol especificado no existe.");

        var worker = new Worker
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            Phone = request.Phone,
            Email = request.Email,
            EmergencyContactName = request.EmergencyContactName,
            EmergencyContactPhone = request.EmergencyContactPhone,
            RFC = request.RFC,
            CURP = request.CURP,
            NSS = request.NSS,
            RoleId = request.RoleId,
            EducationLevel = request.EducationLevel,
            Allergies = request.Allergies,
            ShiftId = request.ShiftId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Workers.Add(worker);
        await _context.SaveChangesAsync();

        // Generar Username
        var firstTwoName = worker.FirstName.Length >= 2
            ? worker.FirstName.Substring(0, 2)
            : worker.FirstName;

        var firstTwoLastName = worker.LastName.Length >= 2
            ? worker.LastName.Substring(0, 2)
            : worker.LastName;

        var username = $"{firstTwoName}{firstTwoLastName}000{worker.Id:D2}".ToUpper();

        var userAccount = new UserAccount
        {
            WorkerId = worker.Id,
            Username = username,
            PasswordHash = request.Password, // SIN HASH por ahora
            RoleId = request.RoleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserAccounts.Add(userAccount);
        await _context.SaveChangesAsync();

        return new CreateWorkerResponse
        {
            WorkerId = worker.Id,
            Username = username,
            Password = request.Password
        };
    }

    //**************************************************************//
    // Metodo para inactivar un trabajador, se busca el trabajador 
    // por su ID
    //**************************************************************//
    public async Task<bool> DeactivateWorkerAsync(int workerId)
    {
        var worker = await _context.Workers
            .Include(w => w.UserAccount)
            .FirstOrDefaultAsync(w => w.Id == workerId && w.IsActive);

        if (worker == null)
            return false;

        worker.IsActive = false;

        // También inactivamos su UserAccount si existe
        if (worker.UserAccount != null)
            worker.UserAccount.IsActive = false;

        await _context.SaveChangesAsync();
        return true;
    }

    //**************************************************************//
    // Metodo para obtener la lista de trabajadores activos, se incluye
    // el nombre completo, el puesto y el estado de actividad
    //**************************************************************//
    public async Task<List<WorkerListResponse>> GetAllAsync()
    {
        return await _context.Workers
            .Include(w => w.Role)
            .Include(w => w.UserAccount)
            .Select(w => new WorkerListResponse
            {
                Id = w.Id,
                FullName = w.FirstName + " " + w.LastName + " " + w.MiddleName,
                Position = w.Role.Name,
                Username = w.UserAccount.Username,
                IsActive = w.IsActive
            })
            .ToListAsync();
    }


    //**************************************************************//
    // Metodo para obtener la lista de trabajadores activos por turno, se incluye
    // el nombre completo, el puesto y el estado de actividad
    //**************************************************************//
    public async Task<List<WorkerListResponse>> GetByShiftAsync(int shiftId)
    {
        return await _context.Workers
            .Include(w => w.Role)
            .Where(w => w.ShiftId == shiftId && w.IsActive)
            .Select(w => new WorkerListResponse
            {
                Id = w.Id,
                FullName = w.FirstName + " " + w.LastName + " " + w.MiddleName,
                Position = w.Role.Name,
                IsActive = w.IsActive
            })
            .ToListAsync();
    }

    //**************************************************************//
    // Metodo para obtener el detalle de un trabajador por su ID, se incluye
    // el nombre completo, el puesto, el turno, la fecha de nacimiento, RFC, CUR
    // y alergias
    //**************************************************************//
    public async Task<WorkerDetailResponse?> GetByIdAsync(int id)
    {
        var worker = await _context.Workers
            .FirstOrDefaultAsync(w => w.Id == id);

        if (worker == null)
            return null;

        return new WorkerDetailResponse
        {
            Id = worker.Id,
            FirstName = worker.FirstName,
            LastName = worker.LastName,
            MiddleName = worker.MiddleName,
            BirthDate = worker.BirthDate,
            Gender = worker.Gender,
            PhotoPath = worker.PhotoPath,
            Phone = worker.Phone,
            Email = worker.Email,
            EmergencyContactName = worker.EmergencyContactName,
            EmergencyContactPhone = worker.EmergencyContactPhone,
            RFC = worker.RFC,
            CURP = worker.CURP,
            NSS = worker.NSS,
            RoleId = worker.RoleId,
            EducationLevel = worker.EducationLevel,
            Allergies = worker.Allergies,
            ShiftId = worker.ShiftId
        };
    }


    //**************************************************************//
    // Metodo para actualizar un trabajador, se valida que el 
    // trabajador exista, que el rol y el turno sean válidos, luego 
    // se actualizan todos los campos permitidos
    //**************************************************************//
    public async Task<bool> UpdateAsync(int id, UpdateWorkerRequest request)
    {
        var worker = await _context.Workers
            .FirstOrDefaultAsync(w => w.Id == id);

        if (worker == null)
            return false;

        // Validar rol
        var roleExists = await _context.Roles
            .AnyAsync(r => r.Id == request.RoleId && r.IsActive);

        if (!roleExists)
            throw new Exception("Rol inválido");

        // Validar turno
        var shiftExists = await _context.Shifts
            .AnyAsync(s => s.Id == request.ShiftId);

        if (!shiftExists)
            throw new Exception("Turno inválido");

        // Actualizar TODOS los campos permitidos
        worker.FirstName = request.FirstName;
        worker.LastName = request.LastName;
        worker.MiddleName = request.MiddleName;
        worker.BirthDate = request.BirthDate;
        worker.Gender = request.Gender;
        worker.PhotoPath = request.PhotoPath;
        worker.Phone = request.Phone;
        worker.Email = request.Email;
        worker.EmergencyContactName = request.EmergencyContactName;
        worker.EmergencyContactPhone = request.EmergencyContactPhone;
        worker.RFC = request.RFC;
        worker.CURP = request.CURP;
        worker.NSS = request.NSS;
        worker.RoleId = request.RoleId;
        worker.EducationLevel = request.EducationLevel;
        worker.Allergies = request.Allergies;
        worker.ShiftId = request.ShiftId;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ActivateWorkerAsync(int id)
    {
        var worker = await _context.Workers.FindAsync(id);

        if (worker == null || worker.IsActive)
            return false;

        worker.IsActive = true;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<WorkerListResponse>> GetActiveAsync()
    {
        return await _context.Workers
            .Where(w => w.IsActive)
            .Include(w => w.Role)
            .Include(w => w.UserAccount) // si ya estás trayendo username
            .Select(w => new WorkerListResponse
            {
                Id = w.Id,
                FullName = w.FirstName + " " + w.LastName + " " + w.MiddleName,
                Position = w.Role.Name,
                IsActive = w.IsActive,
                Username = w.UserAccount != null ? w.UserAccount.Username : null
            })
            .ToListAsync();
    }

    public async Task<List<WorkerListResponse>> GetInactiveAsync()
    {
        return await _context.Workers
            .Where(w => !w.IsActive)
            .Include(w => w.Role)
            .Include(w => w.UserAccount)
            .Select(w => new WorkerListResponse
            {
                Id = w.Id,
                FullName = w.FirstName + " " + w.LastName + " " + w.MiddleName,
                Position = w.Role.Name,
                IsActive = w.IsActive,
                Username = w.UserAccount != null ? w.UserAccount.Username : null
            })
            .ToListAsync();
    }

}
