using TokoApi.DTOs;
using TokoApi.Helpers;

namespace TokoApi.Services;

public interface IProdukService
{
    Task<IEnumerable<ProdukResponse>> GetAllProdukAsync(ProdukQuery query);
    Task<int> CountProdukAsync(ProdukQuery query);
    Task<ProdukResponse> AddProdukAsync(ProdukRequest request);
    Task<ProdukResponse?> UpdateProdukAsync(int id, ProdukRequest request);
    Task<bool> DeleteProdukAsync(int id);
}