using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using MultiHouse.Controllers;
using MultiHouse.Database;
using MultiHouse.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace MultiHouse.Helpers
{
    public static class DataHelper
    {


        public static void InitVariables()
        {
            using (var ctx = new  MHContext())
            {
                HouseController.mainImgPostfix = 10;
                HouseController.imgPostfix = 0;
            }
            
        }
        
        public static void SaveWebImage(string savePath,IFormFile file)
        {
            var fs = file.OpenReadStream();
            Image img = Image.Load(fs);
            fs.Close();
            img.Save(savePath);
        }

        public static House HUploadToHouse(HouseUpload houseUpload)
        {

            return new House()
            {
                Description = houseUpload.Description,
                Address =  houseUpload.Address,
                IsBuying = houseUpload.IsBuying,
                IsRenting = houseUpload.IsRenting,
                MainImg = houseUpload.MainImg.Name,
                RoomCount = houseUpload.RoomCount
            };
        }


        public static bool IsAdminAuthorized(HttpContext ctx)
        {
            return ctx.Request.Cookies["auth_token"] == PersonalData.AuthToken;
        }
        
    }
}