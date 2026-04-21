namespace SampleApp.Models;

internal sealed class Employee
{
    public int EmployeeId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public decimal Salary { get; set; }
}
