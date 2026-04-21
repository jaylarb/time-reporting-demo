namespace SampleApp.Models;

internal sealed class EmployeeBillingSummary
{
    public int EmployeeId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public decimal TotalInvoiced { get; set; }
}
