namespace TokoApi.Models;

public class Kategori
{
    public int Id { get; set; }
    public string NamaKategori { get; set; } = string.Empty;

    // Relasi: Satu kategori punya banyak produk
    public List<Produk> DaftarProduk { get; set; } = new();
}