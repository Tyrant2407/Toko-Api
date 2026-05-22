using System.Collections.Generic;
using System.Threading.Tasks;
using TokoApi.Models;

namespace TokoApi.Repositories;

public interface IKeuanganRepository
{
    Task<IEnumerable<Keuangan>> GetAllAsync();
    Task<Keuangan?> GetByIdAsync(int id);
    Task<Keuangan> AddAsync(Keuangan keuangan);
    Task DeleteAsync(Keuangan keuangan);
}
