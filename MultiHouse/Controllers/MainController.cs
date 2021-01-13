using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        public IActionResult Index([FromForm]HouseSearch houseSearch)
        {
            var houses = _context.Houses2.Select(x => x);
            

            if (houseSearch.Search != null)
            {
                houses = houses.Where(x => x.Address.Contains(houseSearch.Search) ||
                                           x.Description.Contains(houseSearch.Search));
            }

            if (houseSearch.RoomCount != null && houseSearch.RoomCount!=0)
            {
                houses = houses.Where(x => x.RoomCount == houseSearch.RoomCount);
            }

            if (houseSearch.IsBuying != null)
            {
                houses = houses.Where(x => x.IsBuying == houseSearch.IsBuying);
            }

            if (houseSearch.IsRenting != null)
            {
                houses = houses.Where(x => x.IsRenting == houseSearch.IsRenting);
            }
            
            return View(houses.ToList());
        }


        public IActionResult House(int id)
        {
            var house = _context.Houses2
                .Include(x => x.Images)
                .First(x => x.Id == id);
            
            return View(house);
        }


        public IActionResult Search(string id)
        {


            return View("Index");
        }
        
    }
}