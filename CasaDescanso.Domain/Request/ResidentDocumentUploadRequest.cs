using Microsoft.AspNetCore.Http;

public class ResidentDocumentUploadRequest
{
    public IFormFile File { get; set; } = null!;
    public int ResidentId { get; set; }
    public string DocName { get; set; } = string.Empty;
    public string DetalleRecurso { get; set; } = string.Empty;
    public int WorkerId { get; set; }
}