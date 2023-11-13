using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.WebSite.Settings;

[ExcludeFromCodeCoverage]
public class LocalizationSettings
{
    public string DefaultCulture { get; set; }

    public IEnumerable<string> SupportedCultures { get; set; }
}
