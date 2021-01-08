using Microsoft.EntityFrameworkCore;
using MultiHouse.Models;

namespace MultiHouse.Database
{
    public class MHContext : DbContext
    {

        public DbSet<House> Houses { get; set; }
        public DbSet<HouseRequest> HousesRequests { get; set; }
        
        public MHContext(DbContextOptions<MHContext> options) : base(options)
        {
        }

    }
}