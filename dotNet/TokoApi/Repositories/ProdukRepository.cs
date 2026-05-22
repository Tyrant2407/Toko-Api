using Microsoft.EntityFrameworkCore;
using TokoApi.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokoApi.Models;

namespace TokoApi.Repositories;

public class ProdukRepository : IProdukRepository
{
    private readonly AppDbContext _context;

    public ProdukRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produk>> GetAllAsync(ProdukQuery query)
    {
        // 1. Start queryable with Kategori included (needed for mapping NamaKategori)
        var produkQuery = _context.DaftarProduk
            .Include(p => p.Kategori)
            .AsQueryable();

        // 2. Filter by category
        if (query.KategoriId.HasValue)
        {
            produkQuery = produkQuery.Where(p => p.KategoriId == query.KategoriId.Value);
        }

        // 3. Search by name
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            produkQuery = produkQuery.Where(p => p.Nama.Contains(query.Search));
        }

        // 4. Pagination
        var skip = (query.PageNumber - 1) * query.PageSize;

        return await produkQuery
            .OrderBy(p => p.Nama)
            .Skip(skip)
            .Take(query.PageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync(ProdukQuery query)
    {
        var produkQuery = _context.DaftarProduk.AsQueryable();

        if (query.KategoriId.HasValue)
            produkQuery = produkQuery.Where(p => p.KategoriId == query.KategoriId.Value);

        if (!string.IsNullOrWhiteSpace(query.Search))
            produkQuery = produkQuery.Where(p => p.Nama.Contains(query.Search));

        return await produkQuery.CountAsync();
    }

    public async Task<Produk?> GetByIdAsync(int id)
    {
        return await _context.DaftarProduk
            .Include(p => p.Kategori)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Produk produk)
    {
        await _context.DaftarProduk.AddAsync(produk);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Produk produk)
    {
        _context.DaftarProduk.Update(produk);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Produk produk)
    {
        _context.DaftarProduk.Remove(produk);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync()) >= 0;
    }
}