using DemoWebApp.DAL;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.WebSite.Controllers;

public class CategoryController : Controller
{

    public CategoryController()
    { }

    public IActionResult Index()
    {
        var categories = new List<string>();

        return View(categories);
    }
}
