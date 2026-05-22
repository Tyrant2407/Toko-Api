namespace TokoApi.DTOs;

public class ProdukRequest
{
    public string NamaProduk { get; set; } = string.Empty;
    
    // GANTI BARIS INI: Gunakan ID Kategori (Angka), bukan Nama Kategori (Teks)
    public int KategoriId { get; set; } 
    
    public decimal Harga { get; set; }
    public int Stok { get; set; }
    
    public bool IsHalal { get; set; }
    public DateTime? ExpiryDate { get; set; }
}