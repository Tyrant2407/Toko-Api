using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokoApi.DTOs;
using TokoApi.Helpers;
using TokoApi.Models;
using TokoApi.Repositories;

namespace TokoApi.Services;

public class ProdukService : IProdukService
{
    private readonly IProdukRepository _produkRepository;
    private readonly IMapper _mapper;

    public ProdukService(IProdukRepository produkRepository, IMapper mapper)
    {
        _produkRepository = produkRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProdukResponse>> GetAllProdukAsync(ProdukQuery query)
    {
        var produkList = await _produkRepository.GetAllAsync(query);
        return _mapper.Map<IEnumerable<ProdukResponse>>(produkList);
    }

    public async Task<int> CountProdukAsync(ProdukQuery query)
    {
        return await _produkRepository.CountAsync(query);
    }

    public async Task<ProdukResponse> AddProdukAsync(ProdukRequest request)
    {
        var produk = _mapper.Map<Produk>(request);
        await _produkRepository.AddAsync(produk);

        // Reload with Kategori navigation property for proper mapping
        var saved = await _produkRepository.GetByIdAsync(produk.Id);
        return _mapper.Map<ProdukResponse>(saved ?? produk);
    }

    public async Task<ProdukResponse?> UpdateProdukAsync(int id, ProdukRequest request)
    {
        var existingProduk = await _produkRepository.GetByIdAsync(id);
        if (existingProduk == null) return null;

        _mapper.Map(request, existingProduk);
        await _produkRepository.UpdateAsync(existingProduk);

        // Reload for fresh Kategori name
        var updated = await _produkRepository.GetByIdAsync(id);
        return _mapper.Map<ProdukResponse>(updated ?? existingProduk);
    }

    public async Task<bool> DeleteProdukAsync(int id)
    {
        var existingProduk = await _produkRepository.GetByIdAsync(id);
        if (existingProduk == null) return false;

        await _produkRepository.DeleteAsync(existingProduk);
        return true;
    }
}