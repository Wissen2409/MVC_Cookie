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
    public IActionResult Login()
    {


        // sayfa ilk açıldığında, eğer cookie varsa, login ekranı açılmadan başka bir view'a yönlendirme işlemi yapalım!!
        if (Request.Cookies["Email"] != null)
        {
            var email = Request.Cookies["Email"];
            var password = Request.Cookies["Password"];

            if(email==LoginUser.UserName && password==LoginUser.Password){

                return View("Welcome");
            }
        }
        LoginViewModel model = new LoginViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {


        // Kullanıcı kontrolünden geçirilir!!


        // beni hatırla butonu aktif ise cookie oluşturalım!!

        if (model.Email == LoginUser.UserName && model.Password == LoginUser.Password)
        {
            var emailOption = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict   // CRFS saldırılarına karşı koruma!!
            };
            var passwordOption = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict   // CRFS saldırılarına karşı koruma!!
            };
            Response.Cookies.Append("Email", model.Email, emailOption);
            Response.Cookies.Append("Password", model.Password, passwordOption);
            return View("Welcome");

        }
        return View(new LoginViewModel());


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
