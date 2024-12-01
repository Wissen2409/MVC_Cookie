using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
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


    public string CreateSHA512(string inputText)
    {

        SHA512 sha512 = SHA512.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);
        byte[] hashByte = sha512.ComputeHash(inputBytes);
        StringBuilder builder = new StringBuilder();
        foreach (byte b in hashByte){

            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }


    [HttpGet]
    public IActionResult Login()
    {
        // sayfa ilk açıldığında, eğer cookie varsa, login ekranı açılmadan başka bir view'a yönlendirme işlemi yapalım!!
        if (Request.Cookies["Email"] != null)
        {
            var email = Request.Cookies["Email"];
            var password = Request.Cookies["Password"];

            // cookie den hashlenmiş verileri çektik!! bizim sabit email ve password değeri ile karşılaştıracağımız için
            // Email ve password'ude hashlemek gerekmektedir!!

            var shaUserName=CreateSHA512(LoginUser.UserName);
            var shaPassword = CreateSHA512(LoginUser.Password);

            if (email == shaUserName && password == shaPassword)
            {

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


            if (model.RememberMe)
            {

                // Cookie, browserin sizi hatırlaması için kullanılan bir yapıdır,!!
                // Cookie browserin içerisine atılan küçük bir text dosyasıdır!!

                // cookie içerisine birden fazla değer atılabilir!!
                // Cookie atılırken, bazı ayarlar yapılır tmelede güvenlik ayarları kötü niyetli birilerinin yazılımlar sayesinde cookie'nizi çalmaması için temel güvenlik ayarları yapılabilir
                // her cookie'nin bir expire tarihi vardır, Expire date verilen cookie expire date de otomatik olarak silinirler!!
                // cookie atıldıktan sonra, uygulama kapatılsa bile, tekrar tekrar çalıştırdığınızda okunabilir!!
                // cookie, broewser bazlıdır!! bir browserin attığı bir cookie'i başja bir browser okuyamaz!!
                // Cookie içerisinde önemli veriler tutulmamalıdır!!(Veri tabanı şifresi vb) Eğer tutacaksanız, kesinlikle cookie içerisine atılan veri şifrelenmelidir!!


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

                // email ve password verilerini SHA512 ile şifreleyelim 
                var shaEmail=  CreateSHA512(model.Email);
                var shaPassword = CreateSHA512(model.Password);
                Response.Cookies.Append("Email", shaEmail, emailOption);
                Response.Cookies.Append("Password", shaPassword, passwordOption);

            }

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
