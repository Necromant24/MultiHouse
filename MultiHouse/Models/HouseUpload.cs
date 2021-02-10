using Microsoft.AspNetCore.Http;

namespace MultiHouse.Models
{
    public class HouseUpload
    {
        public string Description { get; set; }
        public int RoomCount { get; set; }
        public string Address { get; set; }
        public string IsRenting { get; set; }
        public string IsBuying { get; set; }
        // заставка на иконке в списке на главной
        public IFormFile MainImg { get; set; }

        public IFormFile[] Images { get; set; }
        
        // new fields
        
        public int Cost { get; set; }
        public string Metro { get; set; }
        public int MetroDistance { get; set; }
    }
}