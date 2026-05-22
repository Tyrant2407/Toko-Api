using System.Collections.Generic;
using System.Threading.Tasks;
using TokoApi.Models;

namespace TokoApi.Repositories;

public interface ITransaksiRepository
{
    Task<IEnumerable<Transaksi>> GetKasbonAsync();
    Task<Transaksi?> GetByIdAsync(int id);
    Task UpdateAsync(Transaksi transaksi);
    Task<Transaksi> ProcessCheckoutAsync(Transaksi transaksi, List<Produk> productsToUpdate);
}
