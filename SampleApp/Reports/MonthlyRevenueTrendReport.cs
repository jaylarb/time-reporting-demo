using Dapper;
using Microsoft.Data.Sqlite;
using SampleApp.Models;
using System.Text;

namespace SampleApp.Reports;

internal class MonthlyRevenueTrendReport : ReportBase<IEnumerable<MonthlyRevenueTrendSummary>>
{
    public MonthlyRevenueTrendReport(SqliteConnection connection) : base(connection) { }

    public override string Name => "Monthly Revenue Trend Report";

    protected override IEnumerable<MonthlyRevenueTrendSummary> Build()
    {
        const string sql = """
            SELECT 
                strftime('%Y-%m', invoice_date) AS Month,
                SUM(amount) AS TotalRevenue,
                COUNT(*) AS InvoiceCount
            FROM Invoices
            GROUP BY strftime('%Y-%m', invoice_date)
            ORDER BY Month;
            """;

        return Connection.Query<MonthlyRevenueTrendSummary>(sql);
    }

    protected override string Render(IEnumerable<MonthlyRevenueTrendSummary> model)
    {
        var sb = new StringBuilder();

        foreach (var c in model)
        {
            sb.AppendLine(
                $"{c.Month} - {c.InvoiceCount} - {c.TotalRevenue:C}");
        }

        return sb.ToString();
    }
}
