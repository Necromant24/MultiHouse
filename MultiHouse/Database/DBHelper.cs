using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MultiHouse.Models;

namespace MultiHouse.Database
{
    public static class DBHelper
    {
        
        
        
        
        public static void InsertTestData(MHContext dc)
        {

            bool created = dc.Database.EnsureCreated();

            bool connectable = dc.Database.CanConnect();
            
            dc.Database.Migrate();
        
            InsertTestHouses(dc);
            InsertTestHouseRequests(dc);
        }
        
        public static void InsertTestHouses(MHContext dc)
        {
            
                
                dc.Houses.AddRange(new List<House>()
                {
                    new ()
                    {
                        Description = " desc 1",
                        RoomCount = 2,
                        Address = "большая конюшенная дом 4 кв 4",
                        IsBuying = "",
                        IsRenting = "",
                        MainImg = "h1.jpg"
                    },
                    new ()
                    {
                        Description = " desc 2",
                        RoomCount = 2,
                        Address = "большая морская дом 99 кв 72",
                        IsBuying = "",
                        IsRenting = "",
                        MainImg = "h2.jpg"
                    },
                    new ()
                    {
                        Description = " desc 3",
                        RoomCount = 2,
                        Address = "малая конюшенная дом 1",
                        IsBuying = "",
                        IsRenting = "",
                        MainImg = "h3.jpg"
                    },
                    new ()
                    {
                        Description = " desc 1",
                        RoomCount = 2,
                        Address = "большая конюшенная дом 4",
                        IsBuying = "",
                        IsRenting = "",
                        MainImg = "h4.jpg"
                    },
                    new ()
                    {
                        Description = " desc 1",
                        RoomCount = 5,
                        Address = "большая конюшенная дом 4",
                        IsBuying = "",
                        IsRenting = "",
                        MainImg = "h5.png"
                    },new ()
                    {
                        Description = " desc 1",
                        RoomCount = 4,
                        Address = "большая конюшенная дом 4",
                        IsBuying = "",
                        IsRenting = "",
                        MainImg = "h6.jpg"
                    },
                });
        
                dc.SaveChanges();
            
        }
        
        public static void InsertTestHouseRequests(MHContext dc)
        {
        
            
                dc.HousesRequests.AddRange(new List<HouseRequest>()
                {
                    new()
                    {
                        Address = "",
                        AproximateAddress = "морская",
                        FirstName = "poki",
                        LastName = "afton",
                        Patronymic = "",
                        Phone = "07674876",
                        IsBuying = "",
                        IsRenting = "",
                        RoomCount = 2,
                        Suggestions = "floor less than 3th"
                    },
                    new()
                    {
                        Address = "",
                        AproximateAddress = "конюшенная",
                        FirstName = "Enot",
                        LastName = "Igorev",
                        Patronymic = "",
                        Phone = "245576",
                        IsBuying = "",
                        IsRenting = "",
                        RoomCount = 2,
                        Suggestions = "floor less than 5th"
                    }
                });
        
                dc.SaveChanges();
            
            
        }
        
    }
}