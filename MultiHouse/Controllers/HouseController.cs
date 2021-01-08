using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MultiHouse.Database;
using MultiHouse.Models;

namespace MultiHouse.Controllers
{
    public class HouseController : Controller
    {
        private readonly MHContext _context;

        public HouseController(MHContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        public string Create([FromForm]House house)
        {
            
            //TODO: разобраться с картинками

            _context.Houses.Add(house);
            _context.SaveChanges();

            return "ok";
        }

        public string Delete(int id)
        {
            var house = _context.Houses.First(x => x.Id == id);
            _context.Houses.Remove(house);

            _context.SaveChanges();
            
            return "ok";
        }

        
        


        public void ImgUpload()
        {
            
        }
        
        
        
    }
}