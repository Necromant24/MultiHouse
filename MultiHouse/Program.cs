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

namespace MultiHouse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            RequestController.SendEmail();
            
            
            return;
            CreateDbIfNotExists(host);
            
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
                    
                    
                    
                    //context.Database.Migrate();

                    //context.SaveChanges();

                    bool connected = context.Database.CanConnect();
                    bool created = context.Database.EnsureCreated();
                    

                    if (false)
                    {
                        RelationalDatabaseCreator databaseCreator =
                            (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                        databaseCreator.CreateTables();
                    }
                    
                    
                    
                    //DBHelper.InsertTestData(context);
                
            }
        }

    }
}