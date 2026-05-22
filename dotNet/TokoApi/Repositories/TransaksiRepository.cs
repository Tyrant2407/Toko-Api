using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokoApi.Models;

namespace TokoApi.Repositories;

public class TransaksiRepository : ITransaksiRepository
{
    private readonly AppDbContext _context;

    public TransaksiRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaksi>> GetKasbonAsync()
    {
        return await _context.DaftarTransaksi
            .Include(t => t.DetailTransaksi)
                .ThenInclude(d => d.Produk)
            .Where(t => t.StatusPembayaran != "Lunas")
            .OrderByDescending(t => t.TanggalTransaksi)
            .ToListAsync();
    }

    public async Task<Transaksi?> GetByIdAsync(int id)
    {
        return await _context.DaftarTransaksi
            .Include(t => t.DetailTransaksi)
                .ThenInclude(d => d.Produk)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpdateAsync(Transaksi transaksi)
    {
        _context.DaftarTransaksi.Update(transaksi);
        await _context.SaveChangesAsync();
    }

    public async Task<Transaksi> ProcessCheckoutAsync(Transaksi transaksi, List<Produk> productsToUpdate)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Update stok produk
            foreach (var p in productsToUpdate)
            {
                _context.DaftarProduk.Update(p);
            }

            // 2. Insert transaksi (dan DetailTransaksi melalui nav property)
            await _context.DaftarTransaksi.AddAsync(transaksi);
            
            // 3. Jika Lunas, tambah ke keuangan
            if (transaksi.StatusPembayaran == "Lunas")
            {
                var keuangan = new Keuangan
                {
                    Tipe = "Pemasukan",
                    Nominal = transaksi.TotalHarga,
                    Keterangan = $"Penjualan POS a.n {transaksi.Pembeli}",
                    Tanggal = DateTime.Now
                };
                await _context.DaftarKeuangan.AddAsync(keuangan);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return transaksi;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw; // Re-throw to be handled by the controller
        }
    }
}
