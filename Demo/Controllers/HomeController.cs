using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

public class HomeController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;

    public HomeController(DB db, IWebHostEnvironment en)
    {
        this.db = db;
        this.en = en;
    }

    // GET: Home/Index
    public IActionResult Index()
    {
        return View();
    }
}