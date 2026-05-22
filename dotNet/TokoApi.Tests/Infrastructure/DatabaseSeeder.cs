using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TokoApi;
using TokoApi.Models;

namespace TokoApi.Tests.Infrastructure;

/// <summary>
/// Seeds deterministic, isolated test data into the Testcontainer database.
/// Each test that needs data should call the relevant Seed method and use
/// the returned IDs to build assertions. This keeps tests self-contained.
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds a single Kategori and one Produk. Returns the created Produk ID.
    /// </summary>
    public static async Task<(int ProdukId, int KategoriId, decimal Harga, int InitialStok)> SeedProdukAsync(
        CustomWebApplicationFactory factory,
        string namaProduk = "Kopi Arabika Test",
        decimal harga = 25000m,
        int stok = 50)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var kategori = new Kategori { NamaKategori = $"Kategori-{Guid.NewGuid():N}" };
        db.DaftarKategori.Add(kategori);
        await db.SaveChangesAsync();

        var produk = new Produk
        {
            Nama = $"{namaProduk}-{Guid.NewGuid():N}",
            Harga = harga,
            Stok = stok,
            KategoriId = kategori.Id,
            IsHalal = true
        };
        db.DaftarProduk.Add(produk);
        await db.SaveChangesAsync();

        return (produk.Id, kategori.Id, harga, stok);
    }

    /// <summary>
    /// Queries the real database to get the current Stok value for a product.
    /// Used to verify stock deduction after a checkout.
    /// </summary>
    public static async Task<int> GetProdukStokAsync(CustomWebApplicationFactory factory, int produkId)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var produk = await db.DaftarProduk.AsNoTracking().FirstAsync(p => p.Id == produkId);
        return produk.Stok;
    }

    /// <summary>
    /// Queries the real database to check if a Transaksi was recorded.
    /// </summary>
    public static async Task<Transaksi?> GetTransaksiByPembeliAsync(
        CustomWebApplicationFactory factory, 
        string pembeli)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await db.DaftarTransaksi
            .Include(t => t.DetailTransaksi)
            .FirstOrDefaultAsync(t => t.Pembeli == pembeli);
    }
}
