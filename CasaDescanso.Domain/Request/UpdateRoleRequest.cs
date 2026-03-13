namespace CasaDescanso.Domain.Requests.UpdateRoleRequest;

public class UpdateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
