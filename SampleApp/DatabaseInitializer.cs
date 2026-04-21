using Dapper;
using Microsoft.Data.Sqlite;

namespace SampleApp;

internal class DatabaseInitializer
{
    private readonly SqliteConnection Connection;

    public DatabaseInitializer(SqliteConnection connection)
    {
        Connection = connection;
    }

    public void Initialize()
    {
        InitializeSchema(Connection);
        SeedData(Connection);
    }

    private void InitializeSchema(SqliteConnection connection)
    {
        const string createTablesSql = """
            CREATE TABLE IF NOT EXISTS Employees (
                employee_id INTEGER PRIMARY KEY,
                first_name TEXT NOT NULL,
                last_name TEXT NOT NULL,
                salary REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Invoices (
                invoice_id INTEGER PRIMARY KEY,
                employee_id INTEGER NOT NULL,
                company_id INTEGER NOT NULL,
                amount REAL NOT NULL,
                invoice_date TEXT NOT NULL
            );
            """;

        connection.Execute(createTablesSql);
    }

    private void SeedData(SqliteConnection connection)
    {
        var employeeCount = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM Employees;");
        if (employeeCount == 0)
        {
            var names = GetEmployeeNames();

            var rand = new Random();

            var employees = names
                .Select((n, i) => new
                {
                    employee_id = i + 1,
                    first_name = n.First,
                    last_name = n.Last,
                    salary = rand.Next(40_000, 400_001)
                });

            const string insertEmployeesSql = """
                INSERT INTO Employees (employee_id, first_name, last_name, salary)
                VALUES (@employee_id, @first_name, @last_name, @salary);
                """;

            connection.Execute(insertEmployeesSql, employees);
        }

        var invoiceCount = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM Invoices;");
        if (invoiceCount == 0)
        {
            var rand = new Random();

            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2026, 3, 1);
            var totalDays = (endDate - startDate).Days;

            var invoices = Enumerable.Range(1, 100).Select(i => new
            {
                invoice_id = i,
                employee_id = rand.Next(1, 51),
                company_id = rand.Next(1, 11),
                amount = rand.Next(1_000, 50_001),
                invoice_date = startDate
                    .AddDays(rand.Next(totalDays))
                    .ToString("yyyy-MM-dd")
            });

            const string insertInvoicesSql = """
                INSERT INTO Invoices (invoice_id, employee_id, company_id, amount, invoice_date)
                VALUES (@invoice_id, @employee_id, @company_id, @amount, @invoice_date);
                """;

            connection.Execute(insertInvoicesSql, invoices);
        }
    }

    private (string First, string Last)[] GetEmployeeNames()
    {
        var names = new[]
        {
            ("Liam", "Anderson" ),
            ("Olivia", "Bennett" ),
            ("Noah", "Carter" ),
            ("Emma", "Diaz" ),
            ("Oliver", "Edwards" ),
            ("Ava", "Foster" ),
            ("Elijah", "Garcia" ),
            ("Sophia", "Hughes" ),
            ("William", "Iverson" ),
            ("Isabella", "Jenkins" ),

            ("James", "Kelley" ),
            ("Mia", "Lopez" ),
            ("Benjamin", "Mitchell" ),
            ("Charlotte", "Nguyen" ),
            ("Lucas", "Owens" ),
            ("Amelia", "Parker" ),
            ("Henry", "Quinn" ),
            ("Harper", "Reed" ),
            ("Alexander", "Simmons" ),
            ("Evelyn", "Turner" ),

            ("Michael", "Underwood" ),
            ("Abigail", "Vargas" ),
            ("Daniel", "Walker" ),
            ("Emily", "Xu" ),
            ("Matthew", "Young" ),
            ("Elizabeth", "Zimmerman" ),
            ("Joseph", "Adams" ),
            ("Sofia", "Brooks" ),
            ("Samuel", "Coleman" ),
            ("Avery", "Dunn" ),

            ("David", "Evans" ),
            ("Ella", "Fisher" ),
            ("Carter", "Gray" ),
            ("Scarlett", "Hayes" ),
            ("Wyatt", "Ingram" ),
            ("Grace", "James" ),
            ("John", "Knight" ),
            ("Chloe", "Lane" ),
            ("Jack", "Morris" ),
            ("Victoria", "Nash" ),

            ("Owen", "Ortiz" ),
            ("Lily", "Perez" ),
            ("Luke", "Ramirez" ),
            ("Hannah", "Scott" ),
            ("Jayden", "Taylor" ),
            ("Aria", "Usher" ),
            ("Dylan", "Valdez" ),
            ("Zoey", "White" ),
            ("Levi", "Xiong" ),
            ("Nora", "Yates" )
        };

        return names;
    }
}
