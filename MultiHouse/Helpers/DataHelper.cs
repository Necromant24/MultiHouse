using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using MultiHouse.Controllers;
using MultiHouse.Database;
using MultiHouse.Models;

namespace MultiHouse.Helpers
{
    public static class DataHelper
    {


        public static void InitVariables()
        {
            using (var ctx = new  MHContext())
            {
                HouseController.mainImgPostfix = ctx.HouseImages.OrderBy(x=>x.Id).Last().Id + 1;
                HouseController.imgPostfix = ctx.Houses2.OrderBy(x=>x.Id).Last().Id + 1;
            }
            
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