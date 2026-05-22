using System;

namespace TokoApi.DTOs;

public class KeuanganRequest
{
    public string Tipe { get; set; } = string.Empty; // "Pemasukan" atau "Pengeluaran"
    public decimal Nominal { get; set; }
    public string Keterangan { get; set; } = string.Empty;
    public DateTime? Tanggal { get; set; }
}
