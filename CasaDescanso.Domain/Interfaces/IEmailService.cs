namespace CasaDescanso.Domain.Interfaces;

public interface IEmailService
{
    Task<bool> SendSupportTicketAsync(string subject, string plainTextContent, string htmlContent, string? screenshotBase64 = null);
}
