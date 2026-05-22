using Microsoft.EntityFrameworkCore;
using TokoApi.Models;

namespace TokoApi.Repositories;

public class KategoriRepository : IKategoriRepository
{
    private readonly AppDbContext _context;

    public KategoriRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Kategori>> GetAllAsync()
    {
        return await _context.DaftarKategori
            .Include(k => k.DaftarProduk)
            .ToListAsync();
    }

    public async Task<Kategori?> GetByIdAsync(int id)
    {
        return await _context.DaftarKategori
            .Include(k => k.DaftarProduk)
            .FirstOrDefaultAsync(k => k.Id == id);
    }

    public async Task AddAsync(Kategori kategori)
    {
        await _context.DaftarKategori.AddAsync(kategori);
    }

    public Task UpdateAsync(Kategori kategori)
    {
        _context.DaftarKategori.Update(kategori);
        return Task.CompletedTask;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var kategori = await _context.DaftarKategori.FindAsync(id);
        if (kategori == null) return false;
        _context.DaftarKategori.Remove(kategori);
        return true;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync()) >= 0;
    }
}
