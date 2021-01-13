using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MultiHouse.Controllers
{
    public class AdminController : Controller
    {
        
        public static string AuthToken = "qazwsx";
        public static string Password = "123";

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Panel()
        {
            string password = Request.Form["password"];
            
            

            if (password == Password)
            {
                HttpContext.Response.Cookies.Append("auth_token",AuthToken);
                return View();
            }
            else
            {
                return View("Index");
            }
            
        }
        
    }
}