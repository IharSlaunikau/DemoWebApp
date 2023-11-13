using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class Employee
{
    public int EmployeeId { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Title { get; set; }

    public string TitleOfCourtesy { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? HireDate { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string HomePhone { get; set; }

    public string Extension { get; set; }

    public ICollection<byte> Photo { get; }

    public string Notes { get; set; }

    public int? ReportsTo { get; set; }

    public string PhotoPath { get; set; }

    public Employee ReportsToNavigation { get; set; }

    public ICollection<Employee> InverseReportsToNavigation { get; } = new HashSet<Employee>();

    public ICollection<Order> Orders { get; } = new HashSet<Order>();

    public ICollection<Territory> Territories { get; } = new HashSet<Territory>();
}
