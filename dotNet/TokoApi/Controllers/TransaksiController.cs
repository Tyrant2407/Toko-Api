using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TokoApi.Services;

namespace TokoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Owner,Kasir")]
public class TransaksiController : ControllerBase
{
    private readonly ITransaksiService _transaksiService;

    public TransaksiController(ITransaksiService transaksiService)
    {
        _transaksiService = transaksiService;
    }

    /// <summary>Get all unpaid kasbon. Admin/Owner/Kasir can view.</summary>
    [HttpGet("kasbon")]
    public async Task<IActionResult> GetKasbon()
    {
        var result = await _transaksiService.GetKasbonAsync();
        return Ok(result);
    }

    /// <summary>Mark kasbon as paid (lunas). Admin/Owner/Kasir can process payment.</summary>
    [HttpPut("{id:int}/lunas")]
    public async Task<IActionResult> PelunasanKasbon(int id)
    {
        var success = await _transaksiService.LunasKasbonAsync(id);
        if (!success)
            return BadRequest(new { message = "Transaksi tidak ditemukan atau sudah lunas." });

        return Ok(new { message = "Pelunasan berhasil." });
    }

    /// <summary>Pay partial amount (cicilan) on a kasbon. Admin/Owner/Kasir can process.</summary>
    [HttpPut("{id:int}/cicilan")]
    public async Task<IActionResult> BayarCicilan(int id, [FromBody] TokoApi.DTOs.BayarCicilanRequest request)
    {
        if (request.JumlahBayar <= 0)
            return BadRequest(new { message = "Jumlah bayar harus lebih dari 0." });
        try
        {
            var result = await _transaksiService.BayarCicilanAsync(id, request);
            if (result == null)
                return BadRequest(new { message = "Transaksi tidak ditemukan atau sudah lunas." });
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Checkout (create transaction). Admin/Owner/Kasir can checkout.</summary>
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] TokoApi.DTOs.CheckoutRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _transaksiService.CheckoutAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
