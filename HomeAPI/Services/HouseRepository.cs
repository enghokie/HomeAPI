using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAPI.Models;

// This class tells the controller how to process the HTTP verbs

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
                    var houses = new House[] { }; //Load the houses from the back end data structure

                    ctx.Cache[CacheKey] = houses;
                }
            }
        }

        public House[] GetAllHouses()               // Gets all the houses stored in the cache
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

        public Exception UpdatRooms()               // Finds any rooms that should belong to the house
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                var currentData = ((House[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                bool found = false;
                bool match = false;
                bool noMatch = false;
                RoomRepository roomRepo = new RoomRepository();
                List<Room> roomList = new List<Room>();
                roomList = roomRepo.GetAllRooms().ToList();
                for (int i = 0; i < currentData.Count; i++)
                {
                    for (int j = 0; j < roomList.Count; j++)
                    {
                        if (currentData.ElementAt(i).HouseId == roomList[j].HouseId)    // Checking if the room belongs to the house
                        {
                            if (currentData.ElementAt(i).MyRooms.Count > 0)
                            {
                                for (int k = 0; k < currentData.ElementAt(i).MyRooms.Count; k++)        // Making sure the room is not already there
                                {
                                    if (currentData.ElementAt(i).MyRooms[k].RoomId == roomList[j].RoomId)
                                    {
                                        found = true;
                                        currentData.ElementAt(i).MyRooms[k] = roomList[j];
                                    }
                                }
                                if (!found)
                                    currentData.ElementAt(i).MyRooms.Add(roomList[j]);          // If it is not, add it to the house room list
                            }
                            else
                            {
                                currentData.ElementAt(i).MyRooms.Add(roomList[j]);
                            }
                        }

                        for (int k = 0; k < currentData.ElementAt(i).MyRooms.Count; k++)
                        {
                            if (currentData.ElementAt(i).MyRooms[k].RoomId == roomList[j].RoomId)
                                match = true;

                            if (!match && (k >= currentData.ElementAt(i).MyRooms.Count))
                            {
                                noMatch = true;
                                currentData.ElementAt(i).MyRooms[k] = null;     // If the room doesn't exist anymore, delete it from the list of rooms
                            }
                        }
                    }
                }
                ctx.Cache[CacheKey] = currentData.ToArray();
                if (noMatch || found || !found)
                    return new Exception("updated");
            }
            return new Exception("No rooms found for the houses available");
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
                        {
                            houseList[i] = house;
                            ctx.Cache[CacheKey] = houseList.ToArray();
                            throw new Exception("Already have House: " + house.HouseName +      // Check if the house is already there, if so then update it
                            " updating properties");
                        }
                    }

                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (userList[i].UserName == house.UserName)
                        {
                            houseList.Add(house);
                            added = true;
                        }                                                           //add the new house
                    }
                    if (!added)
                        throw new Exception("Could not find User: " + house.UserName);

                    ctx.Cache[CacheKey] = houseList.ToArray();                //recache the array
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
