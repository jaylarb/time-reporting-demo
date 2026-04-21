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

        runner.RunAll();
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
