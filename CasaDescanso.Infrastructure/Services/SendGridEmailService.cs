using CasaDescanso.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CasaDescanso.Infrastructure.Services;

public class SendGridEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SendGridEmailService> _logger;

    public SendGridEmailService(IConfiguration configuration, ILogger<SendGridEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendSupportTicketAsync(string subject, string plainTextContent, string htmlContent, string? screenshotBase64 = null)
    {
        try
        {
            var apiKey = _configuration["SendGridSettings:ApiKey"];
            var fromEmailStr = _configuration["SendGridSettings:FromEmail"];
            var fromName = _configuration["SendGridSettings:FromName"];
            var supportEmailStr = _configuration["SendGridSettings:SupportEmail"]; // Tu correo

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(fromEmailStr) || string.IsNullOrEmpty(supportEmailStr))
            {
                _logger.LogError("Falta configuración de SendGrid en appsettings.json");
                return false;
            }

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmailStr, fromName);
            var to = new EmailAddress(supportEmailStr, "Francisco (Soporte)");

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (!string.IsNullOrEmpty(screenshotBase64))
            {
                // Limpiar el prefijo si viene como data URI (data:image/png;base64,...)
                var base64Data = screenshotBase64;
                if (screenshotBase64.Contains(","))
                {
                    base64Data = screenshotBase64.Substring(screenshotBase64.IndexOf(",") + 1);
                }
                msg.AddAttachment("CapturaPantalla.png", base64Data, "image/png");
            }
            
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Ticket de soporte enviado correctamente a {SupportEmail}", supportEmailStr);
                return true;
            }
            else
            {
                var body = await response.Body.ReadAsStringAsync();
                _logger.LogError("Error al enviar el correo de SendGrid. Status: {StatusCode}, Body: {Body}", response.StatusCode, body);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción al intentar enviar correo de soporte por SendGrid");
            return false;
        }
    }
}
