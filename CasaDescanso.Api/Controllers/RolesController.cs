using CasaDescanso.Domain.Entities;
using CasaDescanso.Domain.Requests.CreateRoleRequest;
using CasaDescanso.Domain.Requests.UpdateRoleRequest;
using CasaDescanso.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CasaDescanso.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RolesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /api/roles
    [HttpGet("all")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles
            .Where(r => r.IsActive && r.Name != "ADMIN")
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.Description
            })
            .ToListAsync();

        return Ok(roles);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRole(CreateRoleRequest request)
    {
        var exists = await _context.Roles
            .AnyAsync(r => r.Name == request.Name);

        if (exists)
            return BadRequest(new { message = "Ya existe un rol con ese nombre" });

        var role = new Role
        {
            Name = request.Name.Trim().ToUpper(),
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Rol creado correctamente",
            role.Id,
            role.Name
        });
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateRole(int id, UpdateRoleRequest request)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null)
            return NotFound(new { message = "Rol no encontrado" });

        var nameExists = await _context.Roles
            .AnyAsync(r => r.Name == request.Name && r.Id != id);

        if (nameExists)
            return BadRequest(new { message = "Ya existe otro rol con ese nombre" });

        role.Name = request.Name.Trim().ToUpper();
        role.Description = request.Description;
        role.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Rol actualizado correctamente" });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null)
            return NotFound(new { message = "Rol no encontrado" });

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Rol eliminado correctamente" });
    }
}
