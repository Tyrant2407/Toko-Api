using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokoApi.Models;

public class Transaksi
{
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalHarga { get; set; }

    /// <summary>Jumlah yang sudah dibayar (cicilan). Default 0 untuk kasbon baru.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal JumlahDibayar { get; set; } = 0;

    /// <summary>Sisa hutang yang belum dibayar. Computed, not stored.</summary>
    [NotMapped]
    public decimal SisaHutang => TotalHarga - JumlahDibayar;

    public DateTime TanggalTransaksi { get; set; } = DateTime.Now;

    [Required]
    public string Pembeli { get; set; } = string.Empty;

    /// <summary>Status: "Lunas", "Kasbon", atau "Cicilan" (kasbon dengan sebagian bayar)</summary>
    [Required]
    public string StatusPembayaran { get; set; } = "Lunas";

    // Relasi ke Detail
    public ICollection<DetailTransaksi> DetailTransaksi { get; set; } = new List<DetailTransaksi>();
}