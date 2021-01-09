using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
        
        string filePath = "C:/Users/Necromant/RiderProjects/MultiHouse/MultiHouse/Database/ImgPostfix.txt";


        public static int? imgPostfix = null;

        public HouseController(MHContext context)
        {
            _context = context;

            if (imgPostfix == null)
            {
                 string data = System.IO.File.ReadAllText(filePath);
                int postfix = Convert.ToInt32(data);
                imgPostfix = postfix;
            }
            
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

        
        


        public string HouseUpload([FromForm]HouseUpload houseUpload)
        {

            var projectDir = "C:/Users/Necromant/RiderProjects/MultiHouse/MultiHouse/wwwroot/img/houses/";

            
            var fileName ="house"+imgPostfix+".jpg";

            var fullFileName = projectDir + fileName;

            var fs = houseUpload.MainImg.OpenReadStream();

            IImageDecoder decoder = new JpegDecoder();

            Image img = Image.Load(fs,decoder);

            fs.Close();
            
            img.Mutate(x => x.Resize(252, 138));
            
            img.Save(fullFileName);

            House house = DataHelper.HUploadToHouse(houseUpload);

            house.MainImg = fileName;

            _context.Houses.Add(house);
            _context.SaveChanges();

            ++imgPostfix;
            
            System.IO.File.WriteAllText(filePath,imgPostfix.ToString());
            
            
            return "ok";
        }
        
        
        
    }
}