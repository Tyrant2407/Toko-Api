using Microsoft.Extensions.DependencyInjection;
using TokoApi.Repositories;
using TokoApi.Services;

namespace TokoApi.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Registers all application services and repositories for dependency injection.
    /// Called once from Program.cs to keep the startup file clean.
    /// </summary>
    public static void ConfigureApplicationServices(this IServiceCollection services)
    {
        // Produk
        services.AddScoped<IProdukRepository, ProdukRepository>();
        services.AddScoped<IProdukService, ProdukService>();

        // Kategori
        services.AddScoped<IKategoriRepository, KategoriRepository>();
        services.AddScoped<IKategoriService, KategoriService>();

        // Keuangan
        services.AddScoped<IKeuanganRepository, KeuanganRepository>();
        services.AddScoped<IKeuanganService, KeuanganService>();

        // Transaksi
        services.AddScoped<ITransaksiRepository, TransaksiRepository>();
        services.AddScoped<ITransaksiService, TransaksiService>();
    }
}