# Time Reporting Demo

This solution is a small .NET console app that demonstrates a simple reporting pipeline over a local SQLite database. It is intended as a sample/learning project.

## Technology

- .NET 10 (net10.0)
- C#
- Dapper (micro-ORM) for lightweight SQL mapping
- Microsoft.Data.Sqlite for the embedded SQLite provider
- Microsoft.Extensions.DependencyInjection for DI and service registration

## Project layout

- SampleApp/ - Console application project
  - Program.cs - Host setup, DI registrations, and app entry point
  - DatabaseInitializer.cs - Creates schema and seeds sample data into `sampleapp.db`
  - Reports/ - Report implementations and runner
    - IReport.cs - Marker interface used for auto-registration of reports
    - ReportBase.cs - Base class that encapsulates building and rendering reports
    - ReportRunner.cs - Finds all registered IReport instances and writes output to console
    - Several report implementations (EmployeeBillingReport, CompanyBillingReport, MonthlyRevenueTrendReport, RawEmployeeReport, RawInvoiceReport)
  - Models/ - POCOs used to map query results

## Configuration and behavior

- Database
  - The app uses a SQLite file named `sampleapp.db` located in the app base directory (AppContext.BaseDirectory). The file is created automatically.
  - A single SqliteConnection is registered as a singleton and opened at startup.
  - To reset data, stop the app and delete `sampleapp.db`.

- Reports discovery
  - Program.cs scans the executing assembly for types implementing `IReport` and registers each as a transient service. ReportRunner enumerates all IReport instances and executes them.

- Seed data
  - DatabaseInitializer creates `Employees` and `Invoices` tables and seeds them on first run. Employee names are from a fixed list; salaries and invoices are randomly generated within ranges.

## Build and run

Using Visual Studio
- Open the solution and run the SampleApp project.

Using dotnet CLI
- From repository root:
  - dotnet build
  - dotnet run --project SampleApp

## Customization

- Add reports: create a new class implementing `IReport` or inheriting `ReportBase<TModel>`; it will be auto-discovered and run.
- Change seed data or schema: edit `SampleApp/DatabaseInitializer.cs`.
- Use a different database: replace the SqliteConnection registration in `Program.cs` with your provider/connection string and update SQL where needed.

## Notes

- The app intentionally uses minimal dependencies and direct SQL via Dapper to keep the sample simple and focused on reporting concepts.
- Ensure the .NET 10 SDK is installed to build and run the project.