namespace CasaDescanso.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendSupportTicketAsync(string subject, string plainTextContent, string htmlContent);
}
