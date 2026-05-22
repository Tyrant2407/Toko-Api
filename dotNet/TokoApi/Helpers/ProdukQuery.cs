namespace TokoApi.Helpers;

public class ProdukQuery
{
    // Fitur Searching & Filtering
    public string? Search { get; set; } = null;
    public int? KategoriId { get; set; } = null;

    // Fitur Pagination (Kita beri nilai bawaan halaman 1 dan isi 5 data per halaman)
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}