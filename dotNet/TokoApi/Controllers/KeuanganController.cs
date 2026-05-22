using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokoApi.DTOs;
using TokoApi.Services;

namespace TokoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Owner")]
public class KeuanganController : ControllerBase
{
    private readonly IKeuanganService _keuanganService;

    public KeuanganController(IKeuanganService keuanganService)
    {
        _keuanganService = keuanganService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _keuanganService.GetAllKeuanganAsync();
        return Ok(result);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _keuanganService.GetDashboardAnalyticsAsync();
        return Ok(result);
    }

    [HttpPost]
    [Authorize] // Require any authenticated user (will lock to Owner in Module 5)
    public async Task<IActionResult> Add([FromBody] KeuanganRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _keuanganService.AddKeuanganAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _keuanganService.DeleteKeuanganAsync(id);
        if (!success)
            return NotFound(new { message = "Data keuangan tidak ditemukan" });

        return NoContent();
    }
}
