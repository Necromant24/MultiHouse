using Microsoft.AspNetCore.Mvc;

namespace MultiHouse.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Panel()
        {
            string password = Request.Form["password"];

            if (password == "123")
            {
                return View();
            }
            else
            {
                return View("Index");
            }
            
        }
        
    }
}