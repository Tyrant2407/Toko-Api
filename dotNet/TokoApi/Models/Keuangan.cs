using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokoApi.Models;

public class Keuangan
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Tipe { get; set; } = string.Empty; // "Pemasukan" atau "Pengeluaran"

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Nominal { get; set; }

    [Required]
    [StringLength(255)]
    public string Keterangan { get; set; } = string.Empty;

    public DateTime Tanggal { get; set; } = DateTime.Now;
}
