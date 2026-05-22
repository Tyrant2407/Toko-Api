using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using TokoApi.DTOs;
using TokoApi.Models;
using TokoApi.Tests.Infrastructure;
using Xunit;

namespace TokoApi.Tests.IntegrationTests;

[CollectionDefinition("IntegrationTestCollection")]
public class IntegrationTestCollection : ICollectionFixture<CustomWebApplicationFactory>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

[Collection("IntegrationTestCollection")]
public class TransaksiControllerIntegrationTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public TransaksiControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClientWithAuth(role: "Kasir"); // As Kasir role
    }

    [Fact]
    public async Task Checkout_WithKasbon_ShouldDeductStockAndRecordTransaksi()
    {
        // ARRANGE
        // 1. Seed deterministic data in the database
        var pembeliName = $"Budi-{Guid.NewGuid():N}";
        var (produkId, kategoriId, harga, initialStok) = 
            await DatabaseSeeder.SeedProdukAsync(_factory, namaProduk: "Gula Pasir", stok: 10);
        
        var jumlahBeli = 3;

        var request = new CheckoutRequest
        {
            Pembeli = pembeliName,
            StatusPembayaran = "Kasbon",
            Items = new List<CheckoutItemRequest>
            {
                new CheckoutItemRequest { ProdukId = produkId, JumlahBeli = jumlahBeli }
            }
        };

        // ACT
        // 2. Make the HTTP request to the API
        var response = await _client.PostAsJsonAsync("/api/transaksi/checkout", request);

        // 3. Verify HTTP response
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"HTTP Request failed: {response.StatusCode}. Content: {responseContent}");
        }
        response.EnsureSuccessStatusCode(); // Status 200-299
        var result = await response.Content.ReadFromJsonAsync<TransaksiResponse>();
        
        result.Should().NotBeNull();
        result!.Pembeli.Should().Be(pembeliName);
        result.StatusPembayaran.Should().Be("Kasbon");
        result.TotalHarga.Should().Be(harga * jumlahBeli);
        result.JumlahBeli.Should().Be(jumlahBeli);

        // 4. Verify Database State - Stock Deduction
        var currentStok = await DatabaseSeeder.GetProdukStokAsync(_factory, produkId);
        currentStok.Should().Be(initialStok - jumlahBeli);

        // 5. Verify Database State - Transaction Record
        var dbTransaksi = await DatabaseSeeder.GetTransaksiByPembeliAsync(_factory, pembeliName);
        dbTransaksi.Should().NotBeNull();
        dbTransaksi!.StatusPembayaran.Should().Be("Kasbon");
        dbTransaksi.TotalHarga.Should().Be(harga * jumlahBeli);
        dbTransaksi.DetailTransaksi.Should().HaveCount(1);
        
        var detail = dbTransaksi.DetailTransaksi.First();
        detail.ProdukId.Should().Be(produkId);
        detail.JumlahBeli.Should().Be(jumlahBeli);
        detail.HargaSatuan.Should().Be(harga);
        detail.SubTotal.Should().Be(harga * jumlahBeli);
    }
}
