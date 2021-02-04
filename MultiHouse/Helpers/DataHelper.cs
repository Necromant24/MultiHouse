using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Timers;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using MultiHouse.Controllers;
using MultiHouse.Database;
using MultiHouse.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Timer = System.Timers.Timer;

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

        public static async void StartBackupService()
        {

            int minute = 1000 * 60;
            int day = minute * 60 * 24;
            
            Timer timer = new Timer(day);

            //timer.Interval = 1000 * 2;
            
            timer.Elapsed += BDBackup1;
            timer.AutoReset = true;
            timer.Enabled = true;
            
            Thread.Sleep(minute*60*4);

            Timer timer2 = new Timer(day/2);

            //timer.Interval = 1000 * 2;
            
            timer2.Elapsed += BDBackup1;
            timer2.AutoReset = true;
            timer2.Enabled = true;
            
            BDBackup1();
            BDBackup2();
        }

        public static async void BDBackup1(object o = null, ElapsedEventArgs ergs = null)
        {
            var connection = new SqliteConnection("Data Source=wwwroot/database/mh.db");
            connection.Open();
            
            var backup = new SqliteConnection("Data Source=wwwroot/database/mh1.db");
            connection.BackupDatabase(backup);
            
            connection.Close();
            backup.Close();
        }
        
        public static async void BDBackup2(object o = null, ElapsedEventArgs ergs = null)
        {
            var connection = new SqliteConnection("Data Source=wwwroot/database/mh.db");
            connection.Open();
            
            var backup = new SqliteConnection("Data Source=wwwroot/database/mh2.db");
            connection.BackupDatabase(backup);
            
            connection.Close();
            backup.Close();
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