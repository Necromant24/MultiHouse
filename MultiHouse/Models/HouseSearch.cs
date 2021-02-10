namespace MultiHouse.Models
{
    public class HouseSearch
    {
        public string Search { get; set; }
        public string RoomCount { get; set; }
        public string IsRenting { get; set; }
        public string IsBuying { get; set; }
        
        // new fields
        
        public int MaxCost { get; set; }
        public int MinCost { get; set; }
        
        public string Metro { get; set; }
    }
}