using CasaDescanso.Application.Interfaces;
using CasaDescanso.Domain.Request;
using Microsoft.AspNetCore.Mvc;

namespace CasaDescanso.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupportController : ControllerBase
{
    private readonly IEmailService _emailService;

    public SupportController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("ticket")]
    public async Task<IActionResult> CreateTicket([FromBody] SupportTicketRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Description))
            return BadRequest(new { message = "La descripción no puede estar vacía" });

        var subject = $"🚨 NUEVO TICKET DE SOPORTE - Casa Descanso";

        var plainText = $"Ticket de soporte reportado por {request.ReporterName} ({request.ReporterRole}). \n" +
                        $"Pantalla: {request.CurrentUrl}\n" +
                        $"Hora local: {request.LocalTime}\n\n" +
                        $"Descripción del problema:\n{request.Description}";

        var htmlContent = $@"
            <h2>🚨 Nuevo Ticket de Soporte Técnico</h2>
            <hr />
            <p><strong>Reportado por:</strong> {request.ReporterName} ({request.ReporterRole})</p>
            <p><strong>Pantalla actual:</strong> <code>{request.CurrentUrl}</code></p>
            <p><strong>Hora local del reporte:</strong> {request.LocalTime}</p>
            <br />
            <h3>Descripción del problema:</h3>
            <p style='background-color:#f4f4f4; padding: 15px; border-left: 5px solid #d9534f;'>
                {request.Description.Replace("\n", "<br/>")}
            </p>
        ";

        var success = await _emailService.SendSupportTicketAsync(subject, plainText, htmlContent);

        if (success)
            return Ok(new { message = "El reporte ha sido enviado a soporte técnico." });
        else
            return StatusCode(500, new { message = "Error interno al enviar el reporte." });
    }
}
