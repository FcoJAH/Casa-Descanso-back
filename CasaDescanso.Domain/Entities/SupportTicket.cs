namespace CasaDescanso.Domain.Entities;

public class SupportTicket
{
    public int Id { get; set; }
    
    // Almacenamos el UserId del reportero para las notificaciones
    public int ReporterUserId { get; set; }
    
    public string ReporterName { get; set; } = string.Empty;
    public string ReporterRole { get; set; } = string.Empty;
    public string CurrentUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Estados: "Pending", "Resolved"
    public string Status { get; set; } = "Pending";
    
    // Para saber si la campanita ya fue revisada por el usuario
    public bool IsReadByReporter { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
}
