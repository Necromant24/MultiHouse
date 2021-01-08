using Microsoft.AspNetCore.Mvc;

namespace MultiHouse.Controllers
{
    public class AboutController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}