using Microsoft.EntityFrameworkCore;
using TokoApi.Models;

namespace TokoApi;

// Class ini adalah jembatan ke database
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet ini akan menjadi TABEL di database nanti
    public DbSet<Produk> DaftarProduk { get; set; }

    public DbSet<Transaksi> DaftarTransaksi { get; set; }
    public DbSet<DetailTransaksi> DaftarDetailTransaksi { get; set; }

    public DbSet<Kategori> DaftarKategori { get; set; }

    public DbSet<Keuangan> DaftarKeuangan { get; set; }
}