using CasaDescanso.Domain.Request;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly IIncidentService _incidentService;

    public IncidentsController(IIncidentService incidentService)
    {
        _incidentService = incidentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateIncidentRequest request)
    {
        try
        {
            var id = await _incidentService.CreateAsync(request);

            return Ok(new
            {
                message = "Incidencia registrada correctamente",
                incidentId = id
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
        var incident = await _incidentService.GetByIdAsync(id);

        if (incident == null)
            return NotFound(new { message = "Incidencia no encontrada" });

        return Ok(incident);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateIncidentRequest request)
    {
        var updated = await _incidentService.UpdateAsync(id, request);

        if (!updated)
            return NotFound(new { message = "Incidencia no encontrada" });

        return Ok(new { message = "Incidencia actualizada correctamente" });
    }

    [HttpGet("resident/{residentId}")]
    public async Task<IActionResult> GetByResident(int residentId)
    {
        var incidents = await _incidentService.GetByResidentIdAsync(residentId);
        return Ok(incidents);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var incidents = await _incidentService.GetAllAsync();
        return Ok(incidents);
    }

}
