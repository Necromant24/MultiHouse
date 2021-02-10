using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Timer = System.Timers.Timer;

namespace MultiHouse.Helpers
{
    public class Postfixes
    {
        public int MainImgPostfix { get; set; }
        public int ImgPostfix { get; set; }
    }
    
    public static class DataHelper
    {


        public static List<string> MetroList = new List<string>();

        private static string mlist = @"Автово
        Адмиралтейская
            Академическая
        Балтийская
            Бухарестская
        Василеостровская
            Владимирская
        Волковская
            Выборгская
        Горьковская
            Гостиный двор
            Гражданский проспект
            Девяткино
        Достоевская
            Елизаровская
        Звёздная
            Звенигородская
        Кировский завод
        Комендантский проспект
        Крестовский остров
        Купчино
            Ладожская
        Ленинский проспект
        Лесная
            Лиговский проспект
            Ломоносовская
        Маяковская
            Международная
        Московская
            Московские ворота
            Нарвская
        Невский проспект
        Новочеркасская
            Обводный канал
            Обухово
        Озерки
            Парк Победы
            Парнас
        Петроградская
            Пионерская
        Площадь Александра Невского 1
        Площадь Александра Невского 2
        Площадь Восстания
        Площадь Ленина
        Площадь Мужества
        Политехническая
            Приморская
        Пролетарская
            Проспект Большевиков
            Проспект Ветеранов
            Проспект Просвещения
            Пушкинская
        Рыбацкое
            Садовая
        Сенная площадь
        Спасская
            Спортивная
        Старая Деревня
        Технологический институт 1
        Технологический институт 2
        Удельная
            Улица Дыбенко
            Фрунзенская
        Чёрная речка
        Чернышевская
            Чкаловская
        Электросила";


        public static void initMetroList()
        {
            Console.WriteLine(mlist);
            MetroList = mlist.Split('\n').Select(x=>x.Trim()).ToList();
            var d = 7;
        }
        
        
        

        public static void InitVariables()
        {
                string postfixesFile = "wwwroot/database/postfixes.json";
                Postfixes postfixes = JsonConvert.DeserializeObject<Postfixes>(File.ReadAllText(postfixesFile));

                if (postfixes != null)
                {
                    HouseController.imgPostfix = postfixes.ImgPostfix;
                    HouseController.mainImgPostfix = postfixes.MainImgPostfix;
                }
                else
                {
                    HouseController.imgPostfix = 0;
                    HouseController.mainImgPostfix = 0;
                }
                
                
                
                
                
        }

        public static Postfixes LoadPostfixes()
        {
            string postfixesFile = "wwwroot/database/postfixes.json";
            return JsonConvert.DeserializeObject<Postfixes>(File.ReadAllText(postfixesFile));
        }


        public static void SavePosfixes(int mainPostfix, int posfix)
        {
            File.WriteAllText("wwwroot/database/postfixes.json",JsonConvert.SerializeObject(new Postfixes()
            {
                MainImgPostfix = mainPostfix,
                ImgPostfix = posfix
            }));
            
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

            Timer timer2 = new Timer(day/5);

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