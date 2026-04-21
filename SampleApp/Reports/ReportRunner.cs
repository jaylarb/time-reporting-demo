namespace SampleApp.Reports;

internal class ReportRunner
{
    private readonly IEnumerable<IReport> _reports;

    public ReportRunner(IEnumerable<IReport> reports)
    {
        _reports = reports;
    }

    public void RunAll()
    {
        foreach (var report in _reports)
        {
            Console.WriteLine($"===== {report.Name.ToUpper()} =====");
            Console.WriteLine(report);
            Console.WriteLine();
        }
    }
}
