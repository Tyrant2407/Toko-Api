using TokoApi.Models;

namespace TokoApi.Repositories;

public interface IKategoriRepository
{
    Task<IEnumerable<Kategori>> GetAllAsync();
    Task<Kategori?> GetByIdAsync(int id);
    Task AddAsync(Kategori kategori);
    Task UpdateAsync(Kategori kategori);
    Task<bool> DeleteAsync(int id);
    Task<bool> SaveChangesAsync();
}
