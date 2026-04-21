using Dapper;
using Microsoft.Data.Sqlite;
using SampleApp.Models;
using System.Text;

namespace SampleApp.Reports;

internal class RawEmployeeReport : ReportBase<IEnumerable<Employee>>
{
    public RawEmployeeReport(SqliteConnection connection) : base(connection) { }

    public override string Name => "Employee Report";

    protected override IEnumerable<Employee> Build()
    {
        const string sql = """
            SELECT employee_id, first_name, last_name, salary
            FROM Employees
            ORDER BY employee_id;
            """;

        return Connection.Query<Employee>(sql);
    }

    protected override string Render(IEnumerable<Employee> employees)
    {
        var sb = new StringBuilder();

        foreach (var e in employees)
        {
            sb.AppendLine(
                $"{e.EmployeeId}: {e.FirstName} {e.LastName}, Salary: {e.Salary:C}");
        }

        return sb.ToString();
    }
}
