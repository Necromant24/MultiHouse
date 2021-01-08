using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MultiHouse.Database;
using MultiHouse.Models;

namespace MultiHouse.Controllers
{
    public class MainController : Controller
    {
        private readonly MHContext _context;

        public MainController(MHContext context)
        {
            _context = context;
        }


        public IActionResult Index(string? id, [FromForm]HouseSearch search)
        {
            var houses = _context.Houses.Select(x => x);
            
            if (id != null && id != "")
            {
                houses = houses.Where(x => x.Address.Contains(id) ||
                                           x.Description.Contains(id));
            }

            if (search.Search != null)
            {
                houses = houses.Where(x => x.Address.Contains(search.Search) ||
                                           x.Description.Contains(search.Search));
            }

            if (search.RoomCount != null)
            {
                houses = houses.Where(x => x.RoomCount == search.RoomCount);
            }

            if (search.IsBuying != null)
            {
                houses = houses.Where(x => x.IsBuying == search.IsBuying);
            }

            if (search.IsRenting != null)
            {
                houses = houses.Where(x => x.IsRenting == search.IsRenting);
            }
            
            return View(houses.ToList());
        }


        public IActionResult House(int id)
        {
            var house = _context.Houses.First(x => x.Id == id);

            return View(house);
        }


        public IActionResult Search(string id)
        {


            return View("Index");
        }
        
    }
}