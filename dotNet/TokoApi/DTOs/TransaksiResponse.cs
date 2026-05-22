using System;

namespace TokoApi.DTOs;

public class TransaksiResponse
{
    public int Id { get; set; }
    public string Pembeli { get; set; } = string.Empty;
    public decimal TotalHarga { get; set; }
    public string TotalHargaFormatRupiah { get; set; } = string.Empty;
    public decimal JumlahDibayar { get; set; }
    public string JumlahDibayarFormatRupiah { get; set; } = string.Empty;
    public decimal SisaHutang { get; set; }
    public string SisaHutangFormatRupiah { get; set; } = string.Empty;
    public string StatusPembayaran { get; set; } = string.Empty;
    public DateTime TanggalTransaksi { get; set; }

    // Aggregated properties
    public string NamaProduk { get; set; } = string.Empty;
    public int JumlahBeli { get; set; }
}
