using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MultiHouse.Controllers;
using MultiHouse.Database;
using MultiHouse.Helpers;
using MultiHouse.Models;

namespace MultiHouse
{
    public class Program
    {
        public static void Main(string[] args)
        {


            AboutController.AboutModel = About.Load();
            
            
            
            var host = CreateHostBuilder(args).Build();

            PersonalData.AuthToken = Guid.NewGuid().ToString();
            
            CreateDbIfNotExists(host);
            
            DataHelper.InitVariables();
            
            //RequestController.SendEmail();
            
            
            
            // TODO: refactor all code
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
        
        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                    var context = services.GetRequiredService<MHContext>();


                    //var deleted = context.Database.EnsureDeleted();
                    
                    //context.Database.Migrate();

                    //context.SaveChanges();

                    bool connected = context.Database.CanConnect();
                    
                    
                    DBHelper.InsertTestData(context);
                
            }
        }



        void initLocalVariables()
        {
            
        }
        
        

    }
}