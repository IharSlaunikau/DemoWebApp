using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.WebSite.ViewModels;

[ExcludeFromCodeCoverage]
public class ErrorViewModel
{
    public string RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
