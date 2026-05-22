using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokoApi.DTOs;
using TokoApi.Services;

namespace TokoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KategoriController : ControllerBase
{
    private readonly IKategoriService _kategoriService;

    public KategoriController(IKategoriService kategoriService)
    {
        _kategoriService = kategoriService;
    }

    /// <summary>Get all categories. Public — no token required.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _kategoriService.GetAllKategoriAsync();
        return Ok(result);
    }

    /// <summary>Create a new category. Admin/Owner only.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Create([FromBody] KategoriRequest request)
    {
        var result = await _kategoriService.AddKategoriAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    /// <summary>Update a category. Admin/Owner only.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] KategoriRequest request)
    {
        var result = await _kategoriService.UpdateKategoriAsync(id, request);
        if (result == null) return NotFound(new { message = "Kategori tidak ditemukan." });
        return Ok(result);
    }

    /// <summary>Delete a category. Admin/Owner only. Fails if category still has products.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _kategoriService.DeleteKategoriAsync(id);
            if (!success) return NotFound(new { message = "Kategori tidak ditemukan." });
            return Ok(new { message = "Kategori berhasil dihapus." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}