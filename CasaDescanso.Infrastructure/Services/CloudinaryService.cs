using Microsoft.AspNetCore.Http; // Para IFormFile
using CloudinaryDotNet;        // Para Cloudinary y Account
using CloudinaryDotNet.Actions; // Para ImageUploadResult, RawUploadResult, etc.
using Microsoft.Extensions.Options;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> config)
    {
        var settings = config.Value;

        // VALIDACIÓN MANUAL PARA DEPURAR
        if (string.IsNullOrEmpty(settings.CloudName))
        {
            // Si ves este error, significa que builder.Configuration.GetSection no encontró nada
            throw new ArgumentException("ERROR CRÍTICO: El CloudName no se pudo leer del appsettings.json. Revisa el nombre de la sección.");
        }

        var acc = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "expedientes_residentes",
                // Esto ayuda a que el archivo no pierda calidad
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult;
    }

    public async Task<RawUploadResult> UploadPdfAsync(IFormFile file)
    {
        var uploadResult = new RawUploadResult();
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "expedientes_residentes"
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult;
    }

    public async Task<DeletionResult> DeleteFileAsync(string publicId)
    {
        return await _cloudinary.DestroyAsync(new DeletionParams(publicId));
    }
}