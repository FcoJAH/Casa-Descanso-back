namespace CasaDescanso.Infrastructure.Services;

public interface IAuthService
{
    Task<(bool IsSuccess, int UserId, int WorkerId, string FullName, string Position, string ShiftName, bool HasSeenSupportAnnouncement, bool HasSeenCheckinAnnouncement, string Token, string RefreshToken)>
        LoginAsync(string username, string password);

    Task<(bool IsSuccess, int UserId, int WorkerId, string FullName, string Position, string ShiftName, bool HasSeenSupportAnnouncement, bool HasSeenCheckinAnnouncement, string Token, string RefreshToken)>
        RefreshAsync(string token, string refreshToken);

    Task<bool> MarkAnnouncementAsSeenAsync(int userId);
    Task<bool> MarkCheckinAnnouncementAsSeenAsync(int userId);
}
