using Dapper;
using Microsoft.Data.Sqlite;
using SampleApp.Models;
using System.Text;

namespace SampleApp.Reports;

internal class CompanyBillingReport : ReportBase<IEnumerable<CompanyBillingSummary>>
{
    public CompanyBillingReport(SqliteConnection connection) : base(connection) { }

    public override string Name => "Company Billing Report";

    protected override IEnumerable<CompanyBillingSummary> Build()
    {
        const string sql = """
            SELECT 
                i.company_id, SUM(i.amount) AS TotalInvoiced
            FROM Invoices i
            GROUP BY i.company_id
            ORDER BY TotalInvoiced DESC
            LIMIT 10;
            """;

        return Connection.Query<CompanyBillingSummary>(sql);
    }

    protected override string Render(IEnumerable<CompanyBillingSummary> model)
    {
        var sb = new StringBuilder();

        int rank = 1;

        foreach (var c in model)
        {
            sb.AppendLine(
                $"{rank,2}. {c.CompanyId} - {c.TotalInvoiced:C}");

            rank++;
        }

        return sb.ToString();
    }
}
