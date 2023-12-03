using DemoWebApp.WebSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.WebSite.Components;

[ViewComponent(Name = "Breadcrumb")]
public class BreadcrumbsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var currentController = ViewContext.RouteData.Values["controller"].ToString();
        var currentAction = ViewContext.RouteData.Values["action"].ToString();

        var baseUrl = $"{Url.ActionContext.HttpContext.Request.Scheme}://{Url.ActionContext.HttpContext.Request.Host}";

        var breadcrumbs = new List<BreadcrumbModel>
        {
            new BreadcrumbModel { Label = "Home", Url = new Uri($"{baseUrl}{Url.Action("Index", "Home")}") },
            new BreadcrumbModel { Label = currentController, Url = new Uri($"{baseUrl}{Url.Action("Index", currentController)}") },
            new BreadcrumbModel { Label = currentAction }
        };

        return View(breadcrumbs);
    }
}
