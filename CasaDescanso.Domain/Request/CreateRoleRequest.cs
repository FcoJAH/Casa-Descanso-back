namespace CasaDescanso.Domain.Requests.CreateRoleRequest;

public class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
