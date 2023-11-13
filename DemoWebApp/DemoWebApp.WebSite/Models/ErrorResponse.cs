using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.WebSite.Models;

[ExcludeFromCodeCoverage]
public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; }
}
