using Microsoft.AspNetCore.Mvc;
using CasaDescanso.Infrastructure.Services;
using CasaDescanso.Domain.Request.CreateWorkerRequest;
using CasaDescanso.Domain.Requests.UpdateWorkerRequest;

namespace CasaDescanso.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkersController : ControllerBase
{
    private readonly IWorkerService _workerService;

    public WorkersController(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _workerService.CreateWorkerAsync(request);

            return Ok(new
            {
                message = "Trabajador registrado correctamente",
                workerId = result.WorkerId,
                username = result.Username,
                password = result.Password
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivateWorker(int id)
    {
        var result = await _workerService.DeactivateWorkerAsync(id);

        if (!result)
            return NotFound(new { message = "Trabajador no encontrado o ya inactivo" });

        return Ok(new { message = "Trabajador dado de baja correctamente" });
    }

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivateWorker(int id)
    {
        var result = await _workerService.ActivateWorkerAsync(id);

        if (!result)
            return NotFound(new { message = "Trabajador no encontrado o ya activo" });

        return Ok(new { message = "Trabajador activado correctamente" });
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var workers = await _workerService.GetAllAsync();
        return Ok(workers);
    }

    [HttpGet("shift/{shiftId}")]
    public async Task<IActionResult> GetByShift(int shiftId)
    {
        var workers = await _workerService.GetByShiftAsync(shiftId);
        return Ok(workers);
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var worker = await _workerService.GetByIdAsync(id);

        if (worker == null)
            return NotFound(new { message = "Trabajador no encontrado" });

        return Ok(worker);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkerRequest request)
    {
        try
        {
            var result = await _workerService.UpdateAsync(id, request);

            if (!result)
                return NotFound(new { message = "Trabajador no encontrado" });

            return Ok(new { message = "Trabajador actualizado correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveWorkers()
    {
        var workers = await _workerService.GetActiveAsync();
        return Ok(workers);
    }

    [HttpGet("inactive")]
    public async Task<IActionResult> GetInactiveWorkers()
    {
        var workers = await _workerService.GetInactiveAsync();
        return Ok(workers);
    }

}