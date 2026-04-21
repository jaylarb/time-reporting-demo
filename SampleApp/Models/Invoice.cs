namespace SampleApp.Models;

internal sealed class Invoice
{
    public int InvoiceId { get; set; }
    public int EmployeeId { get; set; }
    public int CompanyId { get; set; }
    public decimal Amount { get; set; }
    public DateTime InvoiceDate { get; set; }
}
