namespace SampleApp.Reports;

internal class ReportRunner
{
    private readonly List<IReport> _reports;

    public ReportRunner(IEnumerable<IReport> reports)
    {
        // Capture as a list so we can present stable indexing and reuse instances
        _reports = reports.ToList();
    }

    public int ReportCount => _reports.Count;

    public IReadOnlyList<string> ReportNames => _reports.Select(r => r.Name).ToList().AsReadOnly();

    public void RunAll()
    {
        foreach (var report in _reports)
        {
            PrintReport(report);
        }
    }

    public bool RunByIndex(int index)
    {
        if (index < 0 || index >= _reports.Count)
            return false;

        var report = _reports[index];
        PrintReport(report);
        return true;
    }

    private static void PrintReport(IReport report)
    {
        Console.WriteLine($"===== {report.Name.ToUpper()} =====");
        Console.WriteLine(report);
        Console.WriteLine();
    }
}
