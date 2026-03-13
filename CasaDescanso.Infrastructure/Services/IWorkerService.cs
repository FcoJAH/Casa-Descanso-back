namespace CasaDescanso.Infrastructure.Services;

using CasaDescanso.Domain.Request.CreateWorkerRequest;
using CasaDescanso.Domain.Requests.UpdateWorkerRequest;
using CasaDescanso.Domain.Response.CreateWorkerResponse;
using CasaDescanso.Domain.Responses.WorkerDetailResponse;
using CasaDescanso.Domain.Responses.WorkerListResponse;

public interface IWorkerService
{
    Task<CreateWorkerResponse> CreateWorkerAsync(CreateWorkerRequest request);
    Task<bool> DeactivateWorkerAsync(int workerId);
    Task<List<WorkerListResponse>> GetAllAsync();
    Task<List<WorkerListResponse>> GetByShiftAsync(int shiftId);
    Task<WorkerDetailResponse?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateWorkerRequest request);
    Task<bool> ActivateWorkerAsync(int id);
    Task<List<WorkerListResponse>> GetActiveAsync();
    Task<List<WorkerListResponse>> GetInactiveAsync();
}
