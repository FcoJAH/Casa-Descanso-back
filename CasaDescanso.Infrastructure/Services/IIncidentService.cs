using CasaDescanso.Domain.Request;
using CasaDescanso.Domain.Response;

public interface IIncidentService
{
    Task<int> CreateAsync(CreateIncidentRequest request);
    Task<IncidentDetailResponse?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateIncidentRequest request);
    Task<List<IncidentDetailResponse>> GetByResidentIdAsync(int residentId);
    Task<List<IncidentDetailResponse>> GetAllAsync();
}

