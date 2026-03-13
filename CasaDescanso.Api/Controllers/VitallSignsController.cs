using CasaDescanso.Domain.Request;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VitalSignsController : ControllerBase
{
    private readonly IVitalSignService _service;

    public VitalSignsController(IVitalSignService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVitalSignRequest request)
    {
        try
        {
            var id = await _service.CreateAsync(request);

            return Ok(new
            {
                message = "Signos vitales registrados correctamente",
                vitalSignId = id
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("resident/{residentId}")]
    public async Task<IActionResult> GetByResident(int residentId)
    {
        try
        {
            var records = await _service.GetByResidentAsync(residentId);
            return Ok(records);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

}
