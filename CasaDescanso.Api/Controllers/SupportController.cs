using CasaDescanso.Domain.Interfaces;
using CasaDescanso.Domain.Request;
using CasaDescanso.Infrastructure.Data;
using CasaDescanso.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CasaDescanso.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SupportController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;

    public SupportController(IEmailService emailService, ApplicationDbContext context)
    {
        _emailService = emailService;
        _context = context;
    }

    [HttpPost("ticket")]
    public async Task<IActionResult> CreateTicket([FromBody] SupportTicketRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Description))
            return BadRequest(new { message = "La descripción no puede estar vacía" });

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userId = 0;
        if (userIdClaim != null)
            int.TryParse(userIdClaim, out userId);

        var ticket = new SupportTicket
        {
            ReporterUserId = userId,
            ReporterName = request.ReporterName,
            ReporterRole = request.ReporterRole,
            CurrentUrl = request.CurrentUrl,
            Description = request.Description,
            Status = "Pending",
            IsReadByReporter = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.SupportTickets.Add(ticket);
        await _context.SaveChangesAsync();

        var subject = $"🚨 NUEVO TICKET DE SOPORTE - Casa Descanso";

        var plainText = $"Ticket de soporte reportado por {request.ReporterName} ({request.ReporterRole}). \n" +
                        $"Pantalla: {request.CurrentUrl}\n" +
                        $"Hora local: {request.LocalTime}\n\n" +
                        $"Descripción del problema:\n{request.Description}";

        var htmlContent = $@"
            <h2>🚨 Nuevo Ticket de Soporte Técnico</h2>
            <hr />
            <p><strong>ID Ticket BD:</strong> {ticket.Id}</p>
            <p><strong>Reportado por:</strong> {request.ReporterName} ({request.ReporterRole})</p>
            <p><strong>Pantalla actual:</strong> <code>{request.CurrentUrl}</code></p>
            <p><strong>Hora local del reporte:</strong> {request.LocalTime}</p>
            <br />
            <h3>Descripción del problema:</h3>
            <p style='background-color:#f4f4f4; padding: 15px; border-left: 5px solid #d9534f;'>
                {request.Description.Replace("\n", "<br/>")}
            </p>
        ";

        // El email incluye el pantallazo si existe
        await _emailService.SendSupportTicketAsync(subject, plainText, htmlContent, request.ScreenshotBase64);

        return Ok(new { message = "El reporte ha sido enviado a soporte técnico." });
    }

    // --- ENDPOINTS PARA SISTEMAS ---

    [HttpGet("tickets")]
    [Authorize(Roles = "SISTEMAS")]
    public async Task<IActionResult> GetAllTickets()
    {
        var tickets = await _context.SupportTickets
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return Ok(tickets);
    }

    [HttpPut("tickets/{id}/resolve")]
    [Authorize(Roles = "SISTEMAS")]
    public async Task<IActionResult> ResolveTicket(int id)
    {
        var ticket = await _context.SupportTickets.FindAsync(id);
        if (ticket == null) return NotFound(new { message = "Ticket no encontrado" });

        ticket.Status = "Resolved";
        ticket.ResolvedAt = DateTime.UtcNow;
        // Cuando se resuelve, lo marcamos como NO leído para que al reportero le salga la notificación
        ticket.IsReadByReporter = false; 

        await _context.SaveChangesAsync();
        return Ok(new { message = "Ticket resuelto" });
    }

    // --- ENDPOINTS PARA EL USUARIO NORMAL (Campanita) ---

    [HttpGet("notifications/my-resolved-tickets")]
    public async Task<IActionResult> GetMyResolvedTickets()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized();

        // Obtener los últimos 3 tickets resueltos de este usuario
        var tickets = await _context.SupportTickets
            .Where(t => t.ReporterUserId == userId && t.Status == "Resolved")
            .OrderByDescending(t => t.ResolvedAt)
            .Take(3)
            .Select(t => new {
                t.Id,
                t.Description,
                t.CurrentUrl,
                t.ResolvedAt,
                t.IsReadByReporter
            })
            .ToListAsync();

        return Ok(tickets);
    }

    [HttpPut("notifications/mark-read/{ticketId}")]
    public async Task<IActionResult> MarkTicketAsRead(int ticketId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized();

        var ticket = await _context.SupportTickets.FindAsync(ticketId);
        if (ticket == null || ticket.ReporterUserId != userId) 
            return NotFound();

        ticket.IsReadByReporter = true;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Leído" });
    }
}
