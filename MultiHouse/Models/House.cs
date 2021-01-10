using System.Collections.Generic;

namespace MultiHouse.Models
{
    public class House
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int RoomCount { get; set; }
        public string Address { get; set; }
        public string IsRenting { get; set; }
        public string IsBuying { get; set; }
        // заставка на иконке в списке на главной
        public string MainImg { get; set; }
        public List<HouseImage> Images { get; set; }
        
    }
}