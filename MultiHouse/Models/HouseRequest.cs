namespace MultiHouse.Models
{
    public class HouseRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // отчество
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        
        // доп поле с описанием(текстовым) что хочет дополнительно клиент
        public string Suggestions { get; set; }
        public int RoomCount { get; set; }
        
        // примерный адрес (район) где хочет клиент дом
        public string AproximateAddress { get; set; }
        public string Address { get; set; }
        public string IsRenting { get; set; }
        public string IsBuying { get; set; }

        // not checked || checked || in progress
        public string Status { get; set; } = "not checked";


    }
}