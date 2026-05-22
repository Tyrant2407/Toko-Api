using TokoApi.Helpers;
using TokoApi.Models;

namespace TokoApi.Repositories;

public interface IProdukRepository
{
    Task<IEnumerable<Produk>> GetAllAsync(ProdukQuery query);
    Task<int> CountAsync(ProdukQuery query);
    Task<Produk?> GetByIdAsync(int id);
    Task AddAsync(Produk produk);
    Task UpdateAsync(Produk produk);
    Task DeleteAsync(Produk produk);
    Task<bool> SaveChangesAsync();
}