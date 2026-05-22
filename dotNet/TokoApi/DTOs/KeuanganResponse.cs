using System;

namespace TokoApi.DTOs;

public class KeuanganResponse
{
    public int Id { get; set; }
    public string Tipe { get; set; } = string.Empty;
    public decimal Nominal { get; set; }
    public string NominalFormatRupiah { get; set; } = string.Empty;
    public string Keterangan { get; set; } = string.Empty;
    public DateTime Tanggal { get; set; }
}
