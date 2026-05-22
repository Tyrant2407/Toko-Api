using AutoMapper;
using TokoApi.DTOs;
using TokoApi.Models;
using TokoApi.Repositories;

namespace TokoApi.Services;

public class KategoriService : IKategoriService
{
    private readonly IKategoriRepository _kategoriRepository;
    private readonly IMapper _mapper;

    public KategoriService(IKategoriRepository kategoriRepository, IMapper mapper)
    {
        _kategoriRepository = kategoriRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<KategoriResponse>> GetAllKategoriAsync()
    {
        var list = await _kategoriRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<KategoriResponse>>(list);
    }

    public async Task<KategoriResponse> AddKategoriAsync(KategoriRequest request)
    {
        var kategori = new Kategori { NamaKategori = request.NamaKategori };
        await _kategoriRepository.AddAsync(kategori);
        await _kategoriRepository.SaveChangesAsync();
        return _mapper.Map<KategoriResponse>(kategori);
    }

    public async Task<KategoriResponse?> UpdateKategoriAsync(int id, KategoriRequest request)
    {
        var kategori = await _kategoriRepository.GetByIdAsync(id);
        if (kategori == null) return null;

        kategori.NamaKategori = request.NamaKategori;
        await _kategoriRepository.UpdateAsync(kategori);
        await _kategoriRepository.SaveChangesAsync();
        return _mapper.Map<KategoriResponse>(kategori);
    }

    public async Task<bool> DeleteKategoriAsync(int id)
    {
        var kategori = await _kategoriRepository.GetByIdAsync(id);
        if (kategori == null) return false;

        if (kategori.DaftarProduk?.Any() == true)
            throw new InvalidOperationException(
                $"Kategori \"{kategori.NamaKategori}\" tidak bisa dihapus karena masih memiliki {kategori.DaftarProduk.Count} produk.");

        await _kategoriRepository.DeleteAsync(id);
        await _kategoriRepository.SaveChangesAsync();
        return true;
    }
}
