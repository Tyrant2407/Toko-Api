using System.Collections.Generic;
using System.Threading.Tasks;
using TokoApi.DTOs;

namespace TokoApi.Services;

public interface IKeuanganService
{
    Task<IEnumerable<KeuanganResponse>> GetAllKeuanganAsync();
    Task<KeuanganResponse> AddKeuanganAsync(KeuanganRequest request);
    Task<bool> DeleteKeuanganAsync(int id);
    Task<FinanceDashboardResponse> GetDashboardAnalyticsAsync();
}
