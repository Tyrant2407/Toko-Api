using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokoApi.DTOs;
using TokoApi.Repositories;

namespace TokoApi.Services;

public class TransaksiService : ITransaksiService
{
    private readonly ITransaksiRepository _transaksiRepository;
    private readonly IKeuanganRepository _keuanganRepository;
    private readonly IProdukRepository _produkRepository;
    private readonly IMapper _mapper;

    public TransaksiService(
        ITransaksiRepository transaksiRepository, 
        IKeuanganRepository keuanganRepository, 
        IProdukRepository produkRepository,
        IMapper mapper)
    {
        _transaksiRepository = transaksiRepository;
        _keuanganRepository = keuanganRepository;
        _produkRepository = produkRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransaksiResponse>> GetKasbonAsync()
    {
        var list = await _transaksiRepository.GetKasbonAsync();
        return _mapper.Map<IEnumerable<TransaksiResponse>>(list);
    }

    public async Task<bool> LunasKasbonAsync(int id)
    {
        var transaksi = await _transaksiRepository.GetByIdAsync(id);
        if (transaksi == null || transaksi.StatusPembayaran == "Lunas") return false;

        var sisaBelumDibayar = transaksi.SisaHutang;
        transaksi.JumlahDibayar = transaksi.TotalHarga; // Bayar penuh
        transaksi.StatusPembayaran = "Lunas";
        await _transaksiRepository.UpdateAsync(transaksi);

        // Only record the remaining unpaid amount to cash flow
        if (sisaBelumDibayar > 0)
        {
            await _keuanganRepository.AddAsync(new Models.Keuangan
            {
                Tipe = "Pemasukan",
                Nominal = sisaBelumDibayar,
                Keterangan = $"Pelunasan penuh kasbon a.n {transaksi.Pembeli}",
                Tanggal = System.DateTime.Now
            });
        }
        return true;
    }

    public async Task<TransaksiResponse?> BayarCicilanAsync(int id, BayarCicilanRequest request)
    {
        var transaksi = await _transaksiRepository.GetByIdAsync(id);
        if (transaksi == null || transaksi.StatusPembayaran == "Lunas") return null;

        if (request.JumlahBayar <= 0)
            throw new System.ArgumentException("Jumlah bayar harus lebih dari 0.");

        // Clamp to remaining amount (prevent over-payment)
        var jumlahEfektif = Math.Min(request.JumlahBayar, transaksi.SisaHutang);
        transaksi.JumlahDibayar += jumlahEfektif;

        // Auto-lunas if fully paid
        if (transaksi.SisaHutang <= 0)
        {
            transaksi.JumlahDibayar = transaksi.TotalHarga;
            transaksi.StatusPembayaran = "Lunas";
        }
        else
        {
            transaksi.StatusPembayaran = "Cicilan";
        }

        await _transaksiRepository.UpdateAsync(transaksi);

        // Record the payment to cash flow
        await _keuanganRepository.AddAsync(new Models.Keuangan
        {
            Tipe = "Pemasukan",
            Nominal = jumlahEfektif,
            Keterangan = transaksi.StatusPembayaran == "Lunas"
                ? $"Pelunasan akhir cicilan a.n {transaksi.Pembeli}"
                : $"Cicilan kasbon a.n {transaksi.Pembeli} (Rp{jumlahEfektif:N0} / total Rp{transaksi.TotalHarga:N0})",
            Tanggal = System.DateTime.Now
        });

        var reloaded = await _transaksiRepository.GetByIdAsync(transaksi.Id);
        return _mapper.Map<TransaksiResponse>(reloaded ?? transaksi);
    }

    public async Task<TransaksiResponse> CheckoutAsync(CheckoutRequest request)
    {
        if (request.Items == null || !request.Items.Any())
            throw new System.ArgumentException("Keranjang belanja kosong.");

        var productsToUpdate = new List<Models.Produk>();
        var transaksi = new Models.Transaksi
        {
            Pembeli = request.Pembeli,
            StatusPembayaran = request.StatusPembayaran,
            TanggalTransaksi = System.DateTime.Now,
            TotalHarga = 0,
            DetailTransaksi = new List<Models.DetailTransaksi>()
        };

        foreach (var item in request.Items)
        {
            var produk = await _produkRepository.GetByIdAsync(item.ProdukId);
            if (produk == null)
                throw new System.ArgumentException($"Produk dengan ID {item.ProdukId} tidak ditemukan.");

            if (produk.Stok < item.JumlahBeli)
                throw new System.ArgumentException($"Stok tidak cukup untuk produk '{produk.Nama}'. Sisa stok: {produk.Stok}.");

            // Kurangi stok
            produk.Stok -= item.JumlahBeli;
            productsToUpdate.Add(produk);

            // Buat detail transaksi
            var subTotal = produk.Harga * item.JumlahBeli;
            transaksi.TotalHarga += subTotal;

            transaksi.DetailTransaksi.Add(new Models.DetailTransaksi
            {
                ProdukId = produk.Id,
                JumlahBeli = item.JumlahBeli,
                HargaSatuan = produk.Harga,
                SubTotal = subTotal
            });
        }
        
        if (transaksi.StatusPembayaran == "Lunas")
        {
            transaksi.JumlahDibayar = transaksi.TotalHarga;
        }

        var savedTransaksi = await _transaksiRepository.ProcessCheckoutAsync(transaksi, productsToUpdate);
        
        // Reload to get the Product names in the response
        var reloadedTransaksi = await _transaksiRepository.GetByIdAsync(savedTransaksi.Id);

        return _mapper.Map<TransaksiResponse>(reloadedTransaksi ?? savedTransaksi);
    }
}
