using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TokoApi.DTOs;

namespace TokoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Owner")]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AnalyticsController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    /// <summary>
    /// Formats a decimal as Indonesian Rupiah using InvariantCulture
    /// (safe for Alpine/Globalization-Invariant Docker images).
    /// </summary>
    private static string FormatRupiah(decimal amount)
    {
        var formatted = ((long)Math.Round(amount))
            .ToString("N0", System.Globalization.CultureInfo.InvariantCulture)
            .Replace(",", ".");
        return $"Rp {formatted}";
    }

    /// <summary>
    /// Returns real-time analytics data for the dashboard.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAnalytics()
    {
        var threshold = _configuration.GetValue<int>("LowStockThreshold", 5);

        var produkList = await _context.DaftarProduk.ToListAsync();
        var totalKategori = await _context.DaftarKategori.CountAsync();

        var totalProduk = produkList.Count;
        var totalNilaiAset = produkList.Sum(p => p.Harga * p.Stok);
        var stokKritis = produkList.Count(p => p.Stok < threshold);
        var rataRataHarga = totalProduk > 0 ? produkList.Average(p => (double)p.Harga) : 0;

        var nearExpiryCount = produkList.Count(p => p.ExpiryDate.HasValue && p.ExpiryDate.Value <= DateTime.Now.AddDays(30));

        var result = new AnalyticsResponse
        {
            TotalProduk = totalProduk,
            TotalNilaiAset = totalNilaiAset,
            TotalNilaiAsetFormat = FormatRupiah(totalNilaiAset),
            StokKritis = stokKritis,
            TotalKategori = totalKategori,
            RataRataHarga = (decimal)rataRataHarga,
            RataRataHargaFormat = FormatRupiah((decimal)rataRataHarga),
            NearExpiryCount = nearExpiryCount
        };

        return Ok(result);
    }
}

