namespace TokoApi.DTOs;

public class ProdukResponse
{
    public int Id { get; set; }
    public string NamaProduk { get; set; } = string.Empty;
    public string HargaFormatRupiah { get; set; } = string.Empty;
    public string Kategori { get; set; } = string.Empty;
    public int KategoriId { get; set; }
    public decimal Harga { get; set; }
    public int Stok { get; set; }
    
    public bool IsHalal { get; set; }
    public DateTime? ExpiryDate { get; set; }
}