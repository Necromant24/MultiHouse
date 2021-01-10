using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
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
        
        string filePath = "C:/Users/Necromant/RiderProjects/MultiHouse/MultiHouse/Database/MainImgPostfix.txt";

        string filePath2 = "C:/Users/Necromant/RiderProjects/MultiHouse/MultiHouse/Database/ImgPostfix.txt";


        public static int? mainImgPostfix = null;
        public static int? imgPostfix = null;

        
        static string mainImgSaveDir = "C:/Users/Necromant/RiderProjects/MultiHouse/MultiHouse/wwwroot/img/houses/";
        static string ImgsSaveDir = mainImgSaveDir+"imgs/";
        
        
        
        public HouseController(MHContext context)
        {
            _context = context;

            if (imgPostfix == null)
            {
                 string data = System.IO.File.ReadAllText(filePath);
                int postfix = Convert.ToInt32(data);
                imgPostfix = postfix;
            }
            
            if (mainImgPostfix == null)
            {
                string data = System.IO.File.ReadAllText(filePath2);
                int postfix = Convert.ToInt32(data);
                mainImgPostfix = postfix;
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


            var fileName ="house"+mainImgPostfix+".jpg";
            
            SaveWebImage(houseUpload.MainImg,fileName);

            House house = DataHelper.HUploadToHouse(houseUpload);

            house.MainImg = fileName;

            foreach (var houseImg in houseUpload.Images)
            {
                var imgName = "h" + imgPostfix + ".jpg";
                SaveWebImage(houseImg,imgName);
                house.Images.Add(new HouseImage(){Name = imgName});
                imgPostfix++;
            }


            _context.Houses.Add(house);
            _context.SaveChanges();

            ++imgPostfix;
            
            System.IO.File.WriteAllText(filePath,mainImgPostfix.ToString());
            System.IO.File.WriteAllText(ImgsSaveDir+"ImgPostfix.txt",imgPostfix.ToString());
            
            return "ok";
        }



        public void SaveWebImage(IFormFile file, string name, bool isMain = false)
        {
            var savePath = mainImgSaveDir;
            if (isMain)
            {
                savePath = ImgsSaveDir;
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