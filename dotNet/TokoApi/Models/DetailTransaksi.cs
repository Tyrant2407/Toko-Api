using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokoApi.Models;

public class DetailTransaksi
{
    public int Id { get; set; }

    [Required]
    public int TransaksiId { get; set; }
    
    // Relasi ke Transaksi (Header)
    public Transaksi? Transaksi { get; set; }

    [Required]
    public int ProdukId { get; set; }

    // Relasi ke Produk
    public Produk? Produk { get; set; }

    [Required]
    public int JumlahBeli { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal HargaSatuan { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }
}
