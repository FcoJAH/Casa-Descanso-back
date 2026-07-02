using CasaDescanso.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
                DetalleRecurso = request.DetalleRecurso,
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
                DetalleRecurso = d.DetalleRecurso ?? "",
                d.DocumentUrl,
                d.FileType,
                d.CreatedAt
            })
            .ToListAsync();

        return Ok(documents);
    }

    // Nuevo endpoint para obtener la foto de perfil de un residente
    [HttpGet("{residentId}/profile-photo")]
    public async Task<IActionResult> GetProfilePhoto(int residentId)
    {
        var photo = await _context.ResidentDocuments
            .Where(d => d.ResidentId == residentId && d.DocumentName == "ProfilePhoto")
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new { d.DocumentUrl })
            .FirstOrDefaultAsync();

        if (photo == null)
        {
            return Ok(new { documentUrl = "" });
        }

        return Ok(photo);
    }

    /// <summary>
    /// Endpoint de guardado de foto de perfil
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPatch("{id}/upload-photo")]
    public async Task<IActionResult> UploadPhoto(int id, IFormFile file)
    {
        var resident = await _context.Residents.FindAsync(id);
        if (resident == null) return NotFound("Residente no encontrado");

        if (file == null || file.Length == 0) return BadRequest("No se proporcionó un archivo válido");

        try
        {
            // 1. Subir a Cloudinary (Usando tu servicio ya existente)
            var uploadResult = await _cloudinaryService.UploadImageAsync(file);

            if (uploadResult.Error != null) return BadRequest(uploadResult.Error.Message);

            // 2. Actualizar el campo PhotoPath en la tabla Residents
            resident.PhotoPath = uploadResult.SecureUrl.ToString();

            // 3. Guardar cambios
            _context.Residents.Update(resident);
            await _context.SaveChangesAsync();

            return Ok(new { photoPath = resident.PhotoPath });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }
}
