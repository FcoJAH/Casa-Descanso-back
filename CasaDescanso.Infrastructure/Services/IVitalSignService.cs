using CasaDescanso.Domain.Request;
using CasaDescanso.Domain.Response;

public interface IVitalSignService
{
    Task<int> CreateAsync(CreateVitalSignRequest request);
    Task<List<VitalSignResponse>> GetByResidentAsync(int residentId);
}
