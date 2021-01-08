using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MultiHouse.Database;
using MultiHouse.Models;

namespace MultiHouse.Controllers
{
    public class RequestController : Controller
    {

        private readonly MHContext _context;

        public RequestController(MHContext context)
        {
            _context = context;
        }
        
        
        // GET
        public IActionResult Index()
        {
            return View(_context.HousesRequests.Select(x=>x).ToList());
        }

        public IActionResult RequestView(int id)
        {
            return View(_context.HousesRequests.First(x=>x.Id==id));
        }

        public IActionResult EditStatus()
        {
            int hRequestId = Convert.ToInt32(Request.Query["id"]);
            HouseRequest hr = _context.HousesRequests.First(x => x.Id == hRequestId);

            hr.Status = Request.Form["status"];

            _context.HousesRequests.Update(hr);

            _context.SaveChanges();

            return View("RequestView", hr);
        }
        
    }
}