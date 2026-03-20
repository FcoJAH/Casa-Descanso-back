using CasaDescanso.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ResidentDocumentsController : ControllerBase
{
    private readonly ICloudinaryService _cloudinaryService;
    private readonly ApplicationDbContext _context;

    public ResidentDocumentsController(ICloudinaryService cloudinaryService,
    ApplicationDbContext context)
    {
        _cloudinaryService = cloudinaryService;
        _context = context;
    }

    // Endpoint de prueba para subir un archivo sin asociarlo a un residente
    [HttpPost("upload-test")]
    public async Task<IActionResult> TestUpload(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("Archivo vacío");

        // Probamos si es imagen o PDF
        if (file.ContentType.Contains("pdf"))
        {
            var result = await _cloudinaryService.UploadPdfAsync(file);
            return Ok(new { url = result.SecureUrl, publicId = result.PublicId });
        }
        else
        {
            var result = await _cloudinaryService.UploadImageAsync(file);
            return Ok(new { url = result.SecureUrl, publicId = result.PublicId });
        }
    }

    // Nuevo endpoint para subir un documento y asociarlo a un residente
    [HttpPost("upload-to-resident")]
    public async Task<IActionResult> UploadToResident([FromForm] ResidentDocumentUploadRequest request)
    {
        if (request.File == null || request.File.Length == 0) return BadRequest("No se recibió ningún archivo.");

        var gdlZone = TimeZoneInfo.FindSystemTimeZoneById(OperatingSystem.IsWindows()
            ? "Central Standard Time (Mexico)"
            : "America/Mexico_City");
        var nowGdl = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, gdlZone);

        try
        {
            string url;
            string publicId;

            if (request.File.ContentType.Contains("pdf"))
            {
                var res = await _cloudinaryService.UploadPdfAsync(request.File);
                url = res.SecureUrl.ToString();
                publicId = res.PublicId;
            }
            else
            {
                var res = await _cloudinaryService.UploadImageAsync(request.File);
                url = res.SecureUrl.ToString();
                publicId = res.PublicId;
            }

            var document = new ResidentDocument
            {
                ResidentId = request.ResidentId,
                DocumentName = request.DocName,
                DocumentUrl = url, // La URL que obtienes de Cloudinary
                PublicId = publicId,
                FileType = Path.GetExtension(request.File.FileName).ToLower(),
                CreatedAt = nowGdl,
                CreatedBy = request.WorkerId
            };

            _context.ResidentDocuments.Add(document);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Documento guardado exitosamente", documentId = document.Id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    // Nuevo endpoint para obtener documentos por residente
    [HttpGet("by-resident/{residentId}")]
    public async Task<IActionResult> GetDocumentsByResident(int residentId)
    {
        // Buscamos en la tabla filtrando por el ID del residente
        var documents = await _context.ResidentDocuments
            .Where(d => d.ResidentId == residentId)
            .OrderByDescending(d => d.CreatedAt) // Los más recientes primero
            .Select(d => new
            {
                d.Id,
                d.DocumentName,
                d.DocumentUrl,
                d.FileType,
                d.CreatedAt
            })
            .ToListAsync();

        if (documents == null || !documents.Any())
        {
            return NotFound($"No se encontraron documentos para el residente con ID {residentId}");
        }

        return Ok(documents);
    }
}
