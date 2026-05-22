using AutoMapper;
using System.Linq;
using TokoApi.DTOs;
using TokoApi.Models;

namespace TokoApi.Mappings;

public class MappingProfile : Profile
{
    /// <summary>
    /// Formats a decimal as Indonesian Rupiah without depending on the id-ID culture,
    /// which is unavailable in Alpine/Globalization-Invariant Docker images.
    /// Output example: "Rp 15.000" or "Rp 1.234.567"
    /// </summary>
    private static string FormatRupiah(decimal amount)
    {
        // Format as integer with thousand separator using InvariantCulture,
        // then replace commas (invariant thousand sep) with dots (Indonesian convention).
        var formatted = ((long)Math.Round(amount)).ToString("N0", System.Globalization.CultureInfo.InvariantCulture)
                                                   .Replace(",", ".");
        return $"Rp {formatted}";
    }
    public MappingProfile()
    {
        // Produk -> ProdukResponse
        // Explicitly map Kategori name & Stok, format Harga as Rupiah
        CreateMap<Produk, ProdukResponse>()
            .ForMember(dest => dest.NamaProduk, opt => opt.MapFrom(src => src.Nama))
            .ForMember(dest => dest.Kategori, opt => opt.MapFrom(src => src.Kategori != null ? src.Kategori.NamaKategori : string.Empty))
            .ForMember(dest => dest.HargaFormatRupiah, opt => opt.MapFrom(src => FormatRupiah(src.Harga)))
            .ForMember(dest => dest.Stok, opt => opt.MapFrom(src => src.Stok))
            .ForMember(dest => dest.IsHalal, opt => opt.MapFrom(src => src.IsHalal))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate));

        // ProdukRequest -> Produk (ignore navigation property)
        CreateMap<ProdukRequest, Produk>()
            .ForMember(dest => dest.Nama, opt => opt.MapFrom(src => src.NamaProduk))
            .ForMember(dest => dest.Kategori, opt => opt.Ignore());

        // Kategori -> KategoriResponse
        CreateMap<Kategori, KategoriResponse>()
            .ForMember(dest => dest.JumlahProduk, opt => opt.MapFrom(src => src.DaftarProduk.Count));

        // Keuangan -> KeuanganResponse
        CreateMap<Keuangan, KeuanganResponse>()
            .ForMember(dest => dest.NominalFormatRupiah, opt => opt.MapFrom(src => FormatRupiah(src.Nominal)));

        // KeuanganRequest -> Keuangan
        CreateMap<KeuanganRequest, Keuangan>()
            .ForMember(dest => dest.Tanggal, opt => opt.MapFrom(src => src.Tanggal ?? DateTime.Now));

        // Transaksi -> TransaksiResponse
        CreateMap<Transaksi, TransaksiResponse>()
            .ForMember(dest => dest.TotalHargaFormatRupiah, opt => opt.MapFrom(src => FormatRupiah(src.TotalHarga)))
            .ForMember(dest => dest.JumlahDibayar, opt => opt.MapFrom(src => src.JumlahDibayar))
            .ForMember(dest => dest.JumlahDibayarFormatRupiah, opt => opt.MapFrom(src => FormatRupiah(src.JumlahDibayar)))
            .ForMember(dest => dest.SisaHutang, opt => opt.MapFrom(src => src.SisaHutang))
            .ForMember(dest => dest.SisaHutangFormatRupiah, opt => opt.MapFrom(src => FormatRupiah(src.SisaHutang)))
            .ForMember(dest => dest.NamaProduk, opt => opt.MapFrom(src => string.Join(", ", src.DetailTransaksi.Select(d => d.Produk != null ? $"{d.Produk.Nama} (x{d.JumlahBeli})" : "Unknown"))))
            .ForMember(dest => dest.JumlahBeli, opt => opt.MapFrom(src => src.DetailTransaksi.Sum(d => d.JumlahBeli)));
    }
}