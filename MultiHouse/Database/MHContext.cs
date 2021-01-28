using Microsoft.EntityFrameworkCore;
using MultiHouse.Models;

namespace MultiHouse.Database
{
    public class MHContext : DbContext
    {

        public DbSet<House> Houses2 { get; set; }
        public DbSet<HouseRequest> HousesRequests { get; set; }
        public DbSet<HouseImage> HouseImages { get; set; }
        
        public MHContext(DbContextOptions<MHContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer("Server=.\\sqlexpress;Database=MH2;User Id=MHadmin;password=MHadmin1;Trusted_Connection=False;MultipleActiveResultSets=true;");
            options.UseSqlite("Data Source=wwwroot/database/mh.db");

            
        }
             
        public MHContext()
        {
        }

    }
}