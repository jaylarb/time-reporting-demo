namespace SampleApp.Models;

internal sealed class MonthlyRevenueTrendSummary
{
    public int InvoiceCount { get; set; }
    public required string Month { get; set; }
    public decimal TotalRevenue { get; set; }
}
