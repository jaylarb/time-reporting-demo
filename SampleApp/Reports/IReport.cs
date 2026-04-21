namespace SampleApp.Reports;

// Marker interface so that the app can auto-identify and auto-regiser reports
internal interface IReport
{
    string Name { get; }
}
