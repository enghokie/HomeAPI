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
                    MyRooms = new List<Room>()
                }
            };
        }

        public Exception SaveHouse(House house)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((House[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                    bool added = false;
                    UserRepository tempRepo = new UserRepository();
                    List<User> userList = new List<User>();
                    userList = tempRepo.GetAllUsers().ToList();
                    HouseRepository tempRepo2 = new HouseRepository();
                    List<House> houseList = new List<House>();
                    houseList = tempRepo2.GetAllHouses().ToList();
                    
                    for (int i = 0; i < houseList.Count; i++)
                    {
                        if (houseList[i].HouseId == house.HouseId)
                            throw new Exception("Already have house: " + house.HouseName);
                    }

                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (userList[i].UserName == house.UserName)
                        {
                            currentData.Add(house);
                            added = true;
                        }                                                           //add the new device
                    }
                    if (!added)
                        throw new Exception("Could not find User: " + house.UserName);

                    ctx.Cache[CacheKey] = currentData.ToArray();                //recache the array
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ex;
                }
            }
            return new Exception("none");
        }

        public Exception DeleteHouse(House house)
        {
            var ctx = HttpContext.Current;
            bool found = false;
            if (ctx != null)
            {
                try
                {
                    var currentData = ((House[])ctx.Cache[CacheKey]).ToList();
                    for (int i = 0; i < currentData.Count; i++)
                        if (house.HouseId == currentData.ElementAt(i).HouseId) //search for the matching name to delete
                        {
                           currentData.RemoveAt(i);
                           found = true;
                        }
                    for (int i = 0; i < currentData.Count; i++)
                        System.Diagnostics.Debug.WriteLine(currentData.ElementAt(i).HouseId);  //this serves as a check to see if the item was deleted
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    if (!found)
                        throw new Exception("Could not find House: " + house.HouseName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ex;
                }
            }

            return new Exception("none");
        }
    }
}
