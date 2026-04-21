using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Reports;
using System.Reflection;

namespace SampleApp;

internal class Program
{
    private const string DatabaseFileName = "sampleapp.db";

    private static void Main(string[] args)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        var services = ConfigureServices();

        using var provider = services.BuildServiceProvider();

        var initializer = provider.GetRequiredService<DatabaseInitializer>();
        initializer.Initialize();

        var runner = provider.GetRequiredService<ReportRunner>();

        // Interactive selection loop: list reports and allow the user to choose one to run
        while (true)
        {
            Console.WriteLine("Available reports:");
            Console.WriteLine("0) Run all reports");

            for (int i = 0; i < runner.ReportCount; i++)
            {
                Console.WriteLine($"{i + 1}) {runner.ReportNames[i]}");
            }

            Console.WriteLine("q) Quit");
            Console.Write("Select a report to run: ");

            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine();
                continue;
            }

            input = input.Trim();
            if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
                break;

            if (int.TryParse(input, out var choice))
            {
                if (choice == 0)
                {
                    runner.RunAll();
                    continue;
                }

                var index = choice - 1;
                if (!runner.RunByIndex(index))
                {
                    Console.WriteLine("Invalid selection.");
                }

                continue;
            }

            Console.WriteLine("Invalid input.");
            Console.WriteLine();
        }
    }

    private static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        // Database connection (shared singleton)
        services.AddSingleton(provider =>
        {
            var databasePath = Path.Combine(AppContext.BaseDirectory, DatabaseFileName);

            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = databasePath
            }.ToString();

            var connection = new SqliteConnection(connectionString);
            connection.Open();

            return connection;
        });

        services.AddSingleton<DatabaseInitializer>();

        // Reports
        var reportTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IReport).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var type in reportTypes)
        {
            services.AddTransient(typeof(IReport), type);
        }

        // Runner
        services.AddSingleton<ReportRunner>();

        return services;
    }
}
