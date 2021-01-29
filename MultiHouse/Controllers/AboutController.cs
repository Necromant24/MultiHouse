using Microsoft.AspNetCore.Mvc;
using MultiHouse.Helpers;
using MultiHouse.Models;

namespace MultiHouse.Controllers
{
    public class AboutController : Controller
    {
        public static About AboutModel;
        // GET
        public IActionResult Index()
        {
            return View(AboutModel);
        }


        public IActionResult RedactView()
        {
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }
            
            return View("Redact");
        }



        public IActionResult Redact([FromForm]About about)
        {
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }
            
            about.Save();

            AboutModel = About.Load();

            ViewData["status"] = "успешно изменено";
            return View();
        }
        
    }
}