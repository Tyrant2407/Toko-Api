using System.Collections.Generic;
using System.Threading.Tasks;
using TokoApi.DTOs;

namespace TokoApi.Services;

public interface ITransaksiService
{
    Task<IEnumerable<TransaksiResponse>> GetKasbonAsync();
    Task<bool> LunasKasbonAsync(int id);
    Task<TransaksiResponse?> BayarCicilanAsync(int id, BayarCicilanRequest request);
    Task<TransaksiResponse> CheckoutAsync(CheckoutRequest request);
}
