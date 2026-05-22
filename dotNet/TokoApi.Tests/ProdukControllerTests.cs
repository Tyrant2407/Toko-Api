using Microsoft.AspNetCore.Mvc;
using Moq;
using TokoApi.Controllers;
using TokoApi.DTOs;
using TokoApi.Helpers;
using TokoApi.Services;

namespace TokoApi.Tests;

public class ProdukControllerTests
{
    // ─── Test 1: GetAll returns 200 OK with correct data ──────────────────────
    [Fact]
    public async Task GetAll_HarusMengembalikanOkDanDataProduk()
    {
        // ARRANGE
        var produkDummy = new List<ProdukResponse>
        {
            new ProdukResponse
            {
                Id = 1,
                NamaProduk = "Sepatu Lari",
                Harga = 500000,
                HargaFormatRupiah = "Rp500.000",
                KategoriId = 1,
                Kategori = "Fashion",
                Stok = 10
            }
        };

        var mockService = new Mock<IProdukService>();
        mockService
            .Setup(s => s.GetAllProdukAsync(It.IsAny<ProdukQuery>()))
            .ReturnsAsync(produkDummy);
        mockService
            .Setup(s => s.CountProdukAsync(It.IsAny<ProdukQuery>()))
            .ReturnsAsync(1);

        var controller = new ProdukController(mockService.Object);

        // ACT
        var hasil = await controller.GetAll(new ProdukQuery());

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(hasil);
        var daftarResponse = Assert.IsAssignableFrom<IEnumerable<ProdukResponse>>(okResult.Value);

        var produkPertama = daftarResponse.First();
        Assert.Equal("Sepatu Lari", produkPertama.NamaProduk);
        Assert.Equal("Fashion", produkPertama.Kategori);
        Assert.Equal(10, produkPertama.Stok);
    }

    // ─── Test 2: Update returns 404 when product not found ────────────────────
    [Fact]
    public async Task Update_HarusMengembalikanNotFound_JikaIdTidakAdaDiDb()
    {
        // ARRANGE
        var mockService = new Mock<IProdukService>();
        mockService
            .Setup(s => s.UpdateProdukAsync(It.IsAny<int>(), It.IsAny<ProdukRequest>()))
            .ReturnsAsync((ProdukResponse?)null);

        var controller = new ProdukController(mockService.Object);
        var request = new ProdukRequest { NamaProduk = "Laptop", Harga = 15000000, KategoriId = 1, Stok = 5 };

        // ACT
        var hasil = await controller.Update(999, request);

        // ASSERT
        Assert.IsType<NotFoundObjectResult>(hasil);
    }

    // ─── Test 3: Add returns 201 Created ──────────────────────────────────────
    [Fact]
    public async Task Add_HarusMengembalikanCreated_JikaDataValid()
    {
        // ARRANGE
        var createdProduk = new ProdukResponse
        {
            Id = 5,
            NamaProduk = "Keyboard Mekanikal",
            Harga = 750000,
            HargaFormatRupiah = "Rp750.000",
            KategoriId = 1,
            Kategori = "Elektronik",
            Stok = 20
        };

        var mockService = new Mock<IProdukService>();
        mockService
            .Setup(s => s.AddProdukAsync(It.IsAny<ProdukRequest>()))
            .ReturnsAsync(createdProduk);

        var controller = new ProdukController(mockService.Object);
        var request = new ProdukRequest { NamaProduk = "Keyboard Mekanikal", Harga = 750000, KategoriId = 1, Stok = 20 };

        // ACT
        var hasil = await controller.Add(request);

        // ASSERT
        var createdResult = Assert.IsType<CreatedAtActionResult>(hasil);
        var response = Assert.IsType<ProdukResponse>(createdResult.Value);
        Assert.Equal("Keyboard Mekanikal", response.NamaProduk);
        Assert.Equal(5, response.Id);
    }

    // ─── Test 4: Delete returns 404 when product not found ────────────────────
    [Fact]
    public async Task Delete_HarusMengembalikanNotFound_JikaIdTidakAda()
    {
        // ARRANGE
        var mockService = new Mock<IProdukService>();
        mockService
            .Setup(s => s.DeleteProdukAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

        var controller = new ProdukController(mockService.Object);

        // ACT
        var hasil = await controller.Delete(999);

        // ASSERT
        Assert.IsType<NotFoundObjectResult>(hasil);
    }

    // ─── Test 5: Delete returns 200 OK when product exists ───────────────────
    [Fact]
    public async Task Delete_HarusMengembalikanOk_JikaIdValid()
    {
        // ARRANGE
        var mockService = new Mock<IProdukService>();
        mockService
            .Setup(s => s.DeleteProdukAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        var controller = new ProdukController(mockService.Object);

        // ACT
        var hasil = await controller.Delete(1);

        // ASSERT
        Assert.IsType<OkObjectResult>(hasil);
    }
}