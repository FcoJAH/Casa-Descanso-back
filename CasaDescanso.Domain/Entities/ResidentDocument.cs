using CasaDescanso.Domain.Entities;

public class ResidentDocument
{
    public int Id { get; set; }

    // Relación con el Residente
    public int ResidentId { get; set; }
    
    // Nombre que el usuario le da al archivo (ej: "Acta de Nacimiento 2024")
    public string DocumentName { get; set; } = string.Empty;
    public string DetalleRecurso { get; set; } = string.Empty;

    // La URL segura que nos regresó Cloudinary (HTTPS)
    public string DocumentUrl { get; set; } = string.Empty;

    // ¡IMPORTANTE! El PublicId de Cloudinary. 
    // Lo necesitas si después quieres borrar el archivo de la nube.
    public string PublicId { get; set; } = string.Empty;

    // Tipo de archivo: .pdf, .jpg, .png (útil para poner íconos en el Front)
    public string FileType { get; set; } = string.Empty;

    // Auditoría (Horario Guadalajara)
    public DateTime CreatedAt { get; set; }

    // ID del Worker/Empleado que subió el documento
    public int CreatedBy { get; set; }

    // Propiedad de navegación para Entity Framework
    public virtual Resident? Resident { get; set; }
}