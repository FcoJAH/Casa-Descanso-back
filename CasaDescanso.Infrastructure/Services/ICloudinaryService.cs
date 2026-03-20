using Microsoft.AspNetCore.Http; // Para IFormFile
using CloudinaryDotNet;        // Para Cloudinary y Account
using CloudinaryDotNet.Actions; // Para ImageUploadResult, RawUploadResult, etc.
public interface ICloudinaryService
{
    Task<ImageUploadResult> UploadImageAsync(IFormFile file);
    Task<RawUploadResult> UploadPdfAsync(IFormFile file);
    Task<DeletionResult> DeleteFileAsync(string publicId);
}