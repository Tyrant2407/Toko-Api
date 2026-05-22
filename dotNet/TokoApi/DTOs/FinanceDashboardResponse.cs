namespace TokoApi.DTOs;

public class FinanceDashboardResponse
{
    public decimal DailyRevenue { get; set; }
    public string DailyRevenueFormat { get; set; } = string.Empty;
    public decimal MonthlyRevenue { get; set; }
    public string MonthlyRevenueFormat { get; set; } = string.Empty;
    public decimal TotalExpenses { get; set; }
    public string TotalExpensesFormat { get; set; } = string.Empty;
    public decimal NetProfit { get; set; }
    public string NetProfitFormat { get; set; } = string.Empty;
}
