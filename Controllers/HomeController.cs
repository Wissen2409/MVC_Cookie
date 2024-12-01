using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_Cookie.Models;

namespace MVC_Cookie.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login(){

        LoginViewModel model = new LoginViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model){

        return View();

    }



    public IActionResult Index()
    {
        // Cookie : Web sayfalarının kullanıcıyı tanıması için, kişinin bilgisayarına bıraktığı bir text dosyadır!!
 
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
