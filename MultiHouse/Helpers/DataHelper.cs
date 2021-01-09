using MultiHouse.Models;

namespace MultiHouse.Helpers
{
    public static class DataHelper
    {

        public static House HUploadToHouse(HouseUpload houseUpload)
        {

            return new House()
            {
                Description = houseUpload.Description,
                Address =  houseUpload.Address,
                IsBuying = houseUpload.IsBuying,
                IsRenting = houseUpload.IsRenting,
                MainImg = houseUpload.MainImg.Name,
                RoomCount = houseUpload.RoomCount
            };
        }
        
    }
}