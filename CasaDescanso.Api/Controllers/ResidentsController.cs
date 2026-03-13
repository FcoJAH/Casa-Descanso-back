using CasaDescanso.Domain.Request;
using CasaDescanso.Domain.Request.UpdateResidentRequest;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ResidentsController : ControllerBase
{
    private readonly IResidentService _service;

    public ResidentsController(IResidentService service)
    {
        _service = service;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateResidentRequest request)
    {
        try
        {
            var result = await _service.CreateAsync(request);

            return Ok(new
            {
                message = "Residente registrado correctamente",
                resident = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var resident = await _service.GetByIdAsync(id);

        if (resident == null)
            return NotFound(new { message = "Residente no encontrado" });

        return Ok(resident);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateResident(int id, [FromBody] UpdateResidentRequest dto)
    {
        var updatedResident = await _service.UpdateResidentAsync(id, dto);

        if (updatedResident == null)
            return NotFound(new { message = "Residente no encontrado" });

        return Ok(updatedResident);
    }

    [HttpPatch("{id}/toggle-status")]
    public async Task<IActionResult> ToggleResidentStatus(int id)
    {
        var result = await _service.ToggleActiveAsync(id);

        if (!result)
            return NotFound(new { message = "Residente no encontrado" });

        return Ok(new
        {
            message = "Estado actualizado correctamente"
        });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var residents = await _service.GetAllAsync();
        return Ok(residents);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var residents = await _service.GetActiveAsync();
        return Ok(residents);
    }

    [HttpGet("inactive")]
    public async Task<IActionResult> GetInactive()
    {
        var residents = await _service.GetInactiveAsync();
        return Ok(residents);
    }
}
