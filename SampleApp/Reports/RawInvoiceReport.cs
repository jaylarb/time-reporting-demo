using Dapper;
using Microsoft.Data.Sqlite;
using SampleApp.Models;
using System.Text;

namespace SampleApp.Reports;

internal class RawInvoiceReport : ReportBase<IEnumerable<Invoice>>
{
    public RawInvoiceReport(SqliteConnection connection) : base(connection) { }

    public override string Name => "Invoice Report";

    protected override IEnumerable<Invoice> Build()
    {
        const string sql = """
        SELECT invoice_id, employee_id, company_id, amount, invoice_date
        FROM Invoices
        ORDER BY invoice_id;
        """;

        return Connection.Query<Invoice>(sql);
    }

    protected override string Render(IEnumerable<Invoice> invoices)
    {
        var sb = new StringBuilder();

        foreach (var i in invoices)
        {
            sb.AppendLine(
                $"{i.InvoiceId}: Emp {i.EmployeeId}, Company {i.CompanyId}, {i.Amount:C}, {i.InvoiceDate:d}");
        }

        return sb.ToString();
    }
}
