namespace MultiHouse.Models
{
    public class RequestsFilter
    {
        public string UserName { get; set; }
        public string Status { get; set; } = "не проверено";
        public string ExcludeChecked { get; set; }
        public string Exclude { get; set; }
    }
}