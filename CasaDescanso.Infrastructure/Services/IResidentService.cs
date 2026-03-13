using CasaDescanso.Domain.Request;
using CasaDescanso.Domain.Response;
using CasaDescanso.Domain.Responses;
using CasaDescanso.Domain.Request.UpdateResidentRequest;
using CasaDescanso.Domain.Entities;

public interface IResidentService
{
    Task<ResidentResponse> CreateAsync(CreateResidentRequest request);
    Task<ResidentDetailResponse?> GetByIdAsync(int id);
    Task<Resident?> UpdateResidentAsync(int id, UpdateResidentRequest dto);
    Task<bool> ToggleActiveAsync(int id);
    Task<List<Resident>> GetAllAsync();
    Task<List<Resident>> GetActiveAsync();
    Task<List<Resident>> GetInactiveAsync();

}
