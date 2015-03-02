using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAPI.Models;

namespace HomeAPI.Services
{
    public class HouseRepository
    {
        private const string CacheKey = "HomeStorage";

        public HouseRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var houses = new House[] { }; //Load the houses from the back end data structure??

                    ctx.Cache[CacheKey] = houses;
                }
            }
        }

        public House[] GetAllHouses()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (House[])ctx.Cache[CacheKey];
            }

            return new House[]
            {
            new House
                {
                    HouseName = "PlaceHolder",
                    HouseId = 0,
                    MyRooms = new Room[]{}
                }
            };
        }

        public bool SaveHouse(House house)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((House[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                    RoomRepository tempRepo = new RoomRepository();
                    house.MyRooms = tempRepo.GetAllRooms();
                    currentData.Add(house);                                    //add the new house
                    Console.Write(house.HouseName);
                    for (int i = 0; i < house.MyRooms.Count(); i++)
                    {
                        Console.Write(house.MyRooms[i].RoomName);
                    }
                    ctx.Cache[CacheKey] = currentData.ToArray();                //recache the array

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }

        public bool DeleteHouse(House house)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((House[])ctx.Cache[CacheKey]).ToList();
                    for (int i = 0; i < currentData.Count; i++)
                        if (house.HouseId == currentData.ElementAt(i).HouseId && house.HouseId == currentData.ElementAt(i).HouseId) //search for the matching name to delete
                        {
                           currentData.RemoveAt(i);
                        }
                    for (int i = 0; i < currentData.Count; i++)
                        System.Diagnostics.Debug.WriteLine(currentData.ElementAt(i).HouseId);  //this serves as a check to see if the item was deleted
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }

    }
}
