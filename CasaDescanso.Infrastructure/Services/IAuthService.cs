namespace CasaDescanso.Infrastructure.Services;

public interface IAuthService
{
    Task<(bool IsSuccess, int UserId, int WorkerId, string FullName, string Position, string ShiftName)>
        LoginAsync(string username, string password);
}
