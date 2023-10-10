using DemoWebApp.DAL;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.WebSite.Controllers;

public class ProductController : Controller
{

    public ProductController()
    { }

    public  IActionResult Index()
    {

        return View(new List<string>());
    }
}
