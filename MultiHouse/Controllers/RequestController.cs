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

        public IActionResult Create([FromForm]HouseRequest houseRequest)
        {

            
            _context.HousesRequests.Add(houseRequest);
            _context.SaveChanges();
            
            return View(new HouseRequest());
        }
        
        public IActionResult CreateView(int? id)
        {
            var houseRequest = new HouseRequest();
            if (id != null)
            {
                var house = _context.Houses2.First(x => x.Id == id);

                houseRequest = new HouseRequest()
                {
                    Address = house.Address,
                    RoomCount = house.RoomCount,
                    HouseId = house.Id
                };
            }
            
            return View("Create", houseRequest);
        }
        
    }
}