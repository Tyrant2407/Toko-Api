using TokoApi.DTOs;

namespace TokoApi.Services;

public interface IKategoriService
{
    Task<IEnumerable<KategoriResponse>> GetAllKategoriAsync();
    Task<KategoriResponse> AddKategoriAsync(KategoriRequest request);
    Task<KategoriResponse?> UpdateKategoriAsync(int id, KategoriRequest request);
    Task<bool> DeleteKategoriAsync(int id);
}
