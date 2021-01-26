using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiHouse.Database;
using MultiHouse.Helpers;
using MultiHouse.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace MultiHouse.Controllers
{
    public class HouseController : Controller
    {
        private readonly MHContext _context;
        
        

        public static int? mainImgPostfix = null;
        public static int? imgPostfix = null;

        
        public HouseController(MHContext context)
        {
            _context = context;

        }

        public IActionResult Index([FromQuery]HouseSearch houseSearch)
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

            if (houseSearch.IsBuying == "on")
            {
                houses = houses.Where(x => x.IsBuying == houseSearch.IsBuying);
            }

            if (houseSearch.IsRenting == "on")
            {
                houses = houses.Where(x => x.IsRenting == houseSearch.IsRenting);
            }
            
            return View(houses.ToList());
        }

        public IActionResult Create()
        {
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }
            
            return View();
        }
        
        
        public IActionResult HouseView(int id)
        {
            var house = _context.Houses2
                .Include(x => x.Images)
                .First(x => x.Id == id);
            
            return View(house);
        }


        public string Delete(int id)
        {
            var house = _context.Houses2.First(x => x.Id == id);
            _context.Houses2.Remove(house);

            _context.SaveChanges();
            
            return "ok";
        }

        
        


        public IActionResult HouseUpload([FromForm]HouseUpload houseUpload)
        {
            
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }


            var fileName ="house"+mainImgPostfix+".jpg";
            
            SaveWebImage(houseUpload.MainImg,fileName,true);

            House house = DataHelper.HUploadToHouse(houseUpload);

            house.MainImg = fileName;

            if (house.Images == null)
            {
                house.Images = new List<HouseImage>();
            }

            foreach (var houseImg in houseUpload.Images)
            {
                var imgName = "h" + imgPostfix + ".jpg";
                SaveWebImage(houseImg,imgName);
                house.Images.Add(new HouseImage(){Name = imgName});
                imgPostfix++;
            }


            _context.Houses2.Add(house);
            _context.SaveChanges();

            ++mainImgPostfix;

            return Redirect("/House/Create");
        }



        public void SaveWebImage(IFormFile file, string name, bool isMain = false)
        {
            var savePath = "wwwroot/img/houses/";
            if (isMain)
            {
                savePath += "mainImg/";
            }
            else
            {
                savePath += "imgs/";
            }
            
            var fs = file.OpenReadStream();

            IImageDecoder decoder = new JpegDecoder();

            Image img = Image.Load(fs,decoder);

            fs.Close();
            
            img.Mutate(x => x.Resize(252, 138));
            
            img.Save(savePath+name);
        }
        
        
        
    }
}