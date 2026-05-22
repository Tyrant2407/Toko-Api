using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokoApi.DTOs;
using TokoApi.Models;
using TokoApi.Repositories;

namespace TokoApi.Services;

public class KeuanganService : IKeuanganService
{
    private readonly IKeuanganRepository _keuanganRepository;
    private readonly IMapper _mapper;

    public KeuanganService(IKeuanganRepository keuanganRepository, IMapper mapper)
    {
        _keuanganRepository = keuanganRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Formats a decimal as Indonesian Rupiah using InvariantCulture
    /// (safe for Alpine/Globalization-Invariant Docker images).
    /// </summary>
    private static string FormatRupiah(decimal amount)
    {
        var formatted = ((long)Math.Round(amount))
            .ToString("N0", System.Globalization.CultureInfo.InvariantCulture)
            .Replace(",", ".");
        return $"Rp {formatted}";
    }

    public async Task<IEnumerable<KeuanganResponse>> GetAllKeuanganAsync()
    {
        var keuanganList = await _keuanganRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<KeuanganResponse>>(keuanganList);
    }

    public async Task<KeuanganResponse> AddKeuanganAsync(KeuanganRequest request)
    {
        var keuangan = _mapper.Map<Keuangan>(request);
        var saved = await _keuanganRepository.AddAsync(keuangan);
        return _mapper.Map<KeuanganResponse>(saved);
    }

    public async Task<bool> DeleteKeuanganAsync(int id)
    {
        var existing = await _keuanganRepository.GetByIdAsync(id);
        if (existing == null) return false;

        await _keuanganRepository.DeleteAsync(existing);
        return true;
    }

    public async Task<FinanceDashboardResponse> GetDashboardAnalyticsAsync()
    {
        var allData = await _keuanganRepository.GetAllAsync();
        var today = DateTime.Today;
        var thisMonth = new DateTime(today.Year, today.Month, 1);

        var dailyRevenue = allData
            .Where(k => k.Tipe == "Pemasukan" && k.Tanggal.Date == today)
            .Sum(k => k.Nominal);

        var monthlyRevenue = allData
            .Where(k => k.Tipe == "Pemasukan" && k.Tanggal >= thisMonth)
            .Sum(k => k.Nominal);

        var totalExpenses = allData
            .Where(k => k.Tipe == "Pengeluaran" && k.Tanggal >= thisMonth)
            .Sum(k => k.Nominal);

        var netProfit = monthlyRevenue - totalExpenses;

        var idCulture = System.Globalization.CultureInfo.InvariantCulture;

        return new FinanceDashboardResponse
        {
            DailyRevenue = dailyRevenue,
            DailyRevenueFormat = FormatRupiah(dailyRevenue),
            MonthlyRevenue = monthlyRevenue,
            MonthlyRevenueFormat = FormatRupiah(monthlyRevenue),
            TotalExpenses = totalExpenses,
            TotalExpensesFormat = FormatRupiah(totalExpenses),
            NetProfit = netProfit,
            NetProfitFormat = FormatRupiah(netProfit)
        };
    }
}
