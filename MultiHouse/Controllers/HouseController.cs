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



        string selectLayout(HttpContext ctx)
        {
            if (DataHelper.IsAdminAuthorized(ctx))
            {
                return "_LayoutAdminMH2";
            }
            else
            {
                return "_LayoutMH2";
            }
        }
        
        
        

        public IActionResult Index([FromQuery]HouseSearch houseSearch)
        {
            var houses = _context.Houses2.Select(x => x);



            if (houseSearch.MinCost != null && houseSearch.MinCost>0)
            {
                houses = houses.Where(x => x.Cost >= houseSearch.MinCost);
            }
            
            if (houseSearch.MaxCost != null && houseSearch.MaxCost>0)
            {
                houses = houses.Where(x => x.Cost <= houseSearch.MaxCost);
            }
            
            if (houseSearch.Metro != null && houseSearch.Metro != "" && houseSearch.Metro != " " && DataHelper.MetroList.Contains(houseSearch.Metro))
            {
                houses = houses.Where(x => x.Metro == houseSearch.Metro);
            }

            
            // if (houseSearch.Search != null)
            // {
            //     var keyWords = houseSearch.Search.Split(' ');
            //     foreach (var word in keyWords)
            //     {
            //         if (word != "" && word != " ")
            //         {
            //             houses = houses.Concat(houses.Where(x=>x.Address.Contains(word) ||
            //                                           x.Description.Contains(word)));
            //         }
            //         
            //     }
            // }

            // if (houseSearch.Search != null)
            // {
            //     houses = houses.Where(x => x.Address.Contains(houseSearch.Search) ||
            //                                x.Description.Contains(houseSearch.Search));
            // }

            if (houseSearch.RoomCount != null && houseSearch.RoomCount!="")
            {
                //houses = houses.Where(x => x.RoomCount == houseSearch.RoomCount);

                int rcount = 0;
                bool isNum = int.TryParse(((string?)houseSearch.RoomCount),out rcount);

                if (isNum)
                {
                    houses = houses.Where(x => x.RoomCount == rcount);
                }
                else
                {
                    if (houseSearch.RoomCount == "Студия")
                    {
                        houses = houses.Where(x => x.RoomCount == 0);
                    }
                    else if (houseSearch.RoomCount == "4+")
                    {
                        houses = houses.Where(x => x.RoomCount >= 4);
                    }
                }
                

            }

            if (houseSearch.IsBuying == "on")
            {
                houses = houses.Where(x => x.IsBuying == houseSearch.IsBuying);
            }

            if (houseSearch.IsRenting == "on")
            {
                houses = houses.Where(x => x.IsRenting == houseSearch.IsRenting);
            }


            var houseList = new List<House>();

            
            if (houseSearch.Search != null)
            {
                var keyWords = houseSearch.Search.Split(' ');

                if (keyWords.Length > 10)
                {
                    keyWords = keyWords[..10];
                }

                foreach (var word in keyWords)
                {
                    houseList.AddRange( houses.Where(x => x.Address.Contains(word) ||
                                               x.Description.Contains(word)).ToList());
                }

                if (keyWords.Length > 1)
                {
                    HashSet<House> houseSet = new HashSet<House>(houseList);
                    houseList = houseSet.ToList();
                }
                
            }
            else
            {
                houseList = houses.ToList();
            }

            
            
            
            ViewData["layout"] = selectLayout(HttpContext);

            ViewData["metroList"] = DataHelper.MetroList;
            
            return View(houseList);
        }

        public IActionResult Create()
        {
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }


            ViewData["metroList"] = DataHelper.MetroList;
            
            
            return View();
        }
        
        
        public IActionResult HouseView(int id)
        {
            var house = _context.Houses2
                .Include(x => x.Images)
                .First(x => x.Id == id);

            ViewData["layout"] = selectLayout(HttpContext);
            
            return View(house);
        }


        public IActionResult Delete()
        {
            
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }

            int houseId = Convert.ToInt32(Request.Form["houseId"]);
            
            
            var house = _context.Houses2.First(x => x.Id == houseId);
            _context.Houses2.Remove(house);

            _context.SaveChanges();

            ViewData["status"] = "удален дом с id - "+houseId+"по адресу - "+house.Address;
            
            return View("DeleteView");
        }
        
        
        public IActionResult DeleteView()
        {
            
            bool authorized = Helpers.DataHelper.IsAdminAuthorized(HttpContext);

            if (!authorized)
            {
                return Redirect("/Admin");
            }

            
            return View();
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
                house.Images.Add(new HouseImage(){Name = imgName, HouseId = (int)mainImgPostfix});
                imgPostfix++;
            }


            _context.Houses2.Add(house);
            _context.SaveChanges();

            ++mainImgPostfix;
            
            DataHelper.SavePosfixes((int)mainImgPostfix,(int)imgPostfix);
            
            ViewData["metroList"] = DataHelper.MetroList;

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

            //IImageDecoder decoder = new JpegDecoder();

            Image img = Image.Load(fs);

            fs.Close();
            
            img.Mutate(x => x.Resize(252*2*2, 138*2*2));
            
            img.Save(savePath+name);
        }
        
        
        
    }
}