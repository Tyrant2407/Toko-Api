using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokoApi.DTOs;
using TokoApi.Helpers;
using TokoApi.Services;

namespace TokoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdukController : ControllerBase
{
    private readonly IProdukService _produkService;

    public ProdukController(IProdukService produkService)
    {
        _produkService = produkService;
    }

    /// <summary>
    /// Get all products with optional search, filter, and pagination.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] ProdukQuery query)
    {
        var result = await _produkService.GetAllProdukAsync(query);
        var total = await _produkService.CountProdukAsync(query);

        // Return data + total count in header for frontend pagination (guard for unit tests)
        if (HttpContext != null)
        {
            Response.Headers.Append("X-Total-Count", total.ToString());
            Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
        }

        return Ok(result);
    }

    /// <summary>
    /// Create a new product. Admin only.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Add([FromBody] ProdukRequest request)
    {
        var result = await _produkService.AddProdukAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing product. Admin only.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] ProdukRequest request)
    {
        var result = await _produkService.UpdateProdukAsync(id, request);
        if (result == null) return NotFound(new { Message = "Produk tidak ditemukan" });
        return Ok(result);
    }

    /// <summary>
    /// Delete a product. Admin only.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _produkService.DeleteProdukAsync(id);
        if (!success) return NotFound(new { Message = "Produk tidak ditemukan" });
        return Ok(new { Message = "Produk berhasil dihapus" });
    }
}