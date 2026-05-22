namespace TokoApi.DTOs;

public class AnalyticsResponse
{
    public int TotalProduk { get; set; }
    public decimal TotalNilaiAset { get; set; }
    public string TotalNilaiAsetFormat { get; set; } = string.Empty;
    public int StokKritis { get; set; }
    public int TotalKategori { get; set; }
    public decimal RataRataHarga { get; set; }
    public string RataRataHargaFormat { get; set; } = string.Empty;
    public int NearExpiryCount { get; set; }
}
