using Microsoft.Data.Sqlite;

namespace SampleApp.Reports;

internal abstract class ReportBase<TModel> : IReport
{
    protected readonly SqliteConnection Connection;

    protected ReportBase(SqliteConnection connection)
    {
        Connection = connection;
    }

    public abstract string Name { get; }

    protected abstract TModel Build();
    protected abstract string Render(TModel model);

    public string Execute()
    {
        var model = Build();
        return Render(model);
    }

    public override string ToString() => Execute();
}
