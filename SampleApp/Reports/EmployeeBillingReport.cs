using Dapper;
using Microsoft.Data.Sqlite;
using SampleApp.Models;
using System.Text;

namespace SampleApp.Reports;

internal class EmployeeBillingReport : ReportBase<IEnumerable<EmployeeBillingSummary>>
{
    public EmployeeBillingReport(SqliteConnection connection) : base(connection) { }

    public override string Name => "Employee Billing Report";

    protected override IEnumerable<EmployeeBillingSummary> Build()
    {
        const string sql = """
            SELECT 
                e.employee_id, e.first_name, e.last_name, SUM(i.amount) AS TotalInvoiced
            FROM Employees e
            JOIN Invoices i ON i.employee_id = e.employee_id
            GROUP BY e.employee_id, e.first_name, e.last_name
            ORDER BY TotalInvoiced DESC
            LIMIT 10;
            """;

        return Connection.Query<EmployeeBillingSummary>(sql);
    }

    protected override string Render(IEnumerable<EmployeeBillingSummary> model)
    {
        var sb = new StringBuilder();

        int rank = 1;

        foreach (var e in model)
        {
            sb.AppendLine(
                $"{rank,2}. {e.FirstName} {e.LastName} (ID: {e.EmployeeId}) - {e.TotalInvoiced:C}");

            rank++;
        }

        return sb.ToString();
    }
}
