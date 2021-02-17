using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using MultiHouse.Helpers;
using Newtonsoft.Json;

namespace MultiHouse.Models
{
    public class About
    {
        public string Head { get; set; }
        public string Label1 { get; set; }
        public string Label2 { get; set; }
        public string Label3 { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        public IFormFile[] Images { get; set; }

        public int LabelPostfix1 { get; set; } = 0;
        public int LabelPostfix2 { get; set; } = 0;
        public int LabelPostfix3 { get; set; } = 0;


        public void Save()
        {
            

            if (Images != null)
            {
                if (Images.Length == 3)
                {
                    DataHelper.SaveWebImage("wwwroot/img/About/img1.jpg",Images[0]);
                    DataHelper.SaveWebImage("wwwroot/img/About/img2.jpg",Images[1]);
                    DataHelper.SaveWebImage("wwwroot/img/About/img3.jpg",Images[2]);
                }
                
            }
            
            

            // ++LabelPostfix1;
            // ++LabelPostfix2;
            // ++LabelPostfix3;
            
            // DataHelper.SaveWebImage("wwwroot/img/About/l1_img"+LabelPostfix1+".jpg",Images[0]);
            // DataHelper.SaveWebImage("wwwroot/img/About/l2_img"+LabelPostfix2+".jpg",Images[1]);
            // DataHelper.SaveWebImage("wwwroot/img/About/l3_img"+LabelPostfix3+".jpg",Images[2]);
            
            
            
            File.WriteAllText("database/About.json", JsonConvert.SerializeObject(this));
        }

        public static About Load()
        {
            return JsonConvert.DeserializeObject<About>(File.ReadAllText("database/About.json"));
        }
        
    }
}