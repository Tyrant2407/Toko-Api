using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokoApi.Models;

namespace TokoApi.Repositories;

public class KeuanganRepository : IKeuanganRepository
{
    private readonly AppDbContext _context;

    public KeuanganRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Keuangan>> GetAllAsync()
    {
        return await _context.DaftarKeuangan
            .OrderByDescending(k => k.Tanggal)
            .ToListAsync();
    }

    public async Task<Keuangan?> GetByIdAsync(int id)
    {
        return await _context.DaftarKeuangan.FindAsync(id);
    }

    public async Task<Keuangan> AddAsync(Keuangan keuangan)
    {
        _context.DaftarKeuangan.Add(keuangan);
        await _context.SaveChangesAsync();
        return keuangan;
    }

    public async Task DeleteAsync(Keuangan keuangan)
    {
        _context.DaftarKeuangan.Remove(keuangan);
        await _context.SaveChangesAsync();
    }
}
