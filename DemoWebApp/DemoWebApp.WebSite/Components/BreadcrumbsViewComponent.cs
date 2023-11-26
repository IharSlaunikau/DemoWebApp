using DemoWebApp.WebSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.WebSite.Components;

public class BreadcrumbsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IEnumerable<BreadcrumbModel> items)
    {
        return View(items);
    }
}
