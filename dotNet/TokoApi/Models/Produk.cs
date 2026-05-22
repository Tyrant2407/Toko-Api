using System.ComponentModel.DataAnnotations;

namespace TokoApi.Models;

public class Produk
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama produk tidak boleh kosong!")]
    [StringLength(100, ErrorMessage = "Nama produk terlalu panjang, maksimal 100 karakter.")]
    public string Nama { get; set; } = string.Empty;

    [Range(1000, 100000000, ErrorMessage = "Harga produk minimal Rp 1.000 dan maksimal Rp 100.000.000")]
    public decimal Harga { get; set; }

    [Required]
    public int KategoriId { get; set; }
    public int Stok { get; set; } // <-- Tambahkan ini di model Produk.cs kamu
    
    // UMKM Specific Attributes
    public bool IsHalal { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public Kategori? Kategori { get; set; } 
}