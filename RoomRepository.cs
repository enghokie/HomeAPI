using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAPI.Models;

namespace HomeAPI.Services
{
    public class RoomRepository
    {
        private const string CacheKey = "RoomStore";

        public RoomRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var rooms = new Room[] { }; //Load the rooms from the back end data structure??

                    ctx.Cache[CacheKey] = rooms;
                }
            }
        }

        public Room[] GetAllRooms()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (Room[])ctx.Cache[CacheKey];
            }

            return new Room[]
            {
            new Room
                {
                    RoomName = "PlaceHolder",
                    RoomId = 0,
                    MyDevices = new Device[]{}
                }
            };
        }

        public bool SaveRoom(Room room)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Room[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                    DeviceRepository tempRepo = new DeviceRepository();
                    room.MyDevices = tempRepo.GetAllDevices();
                    currentData.Add(room);          //add the new room
                    Console.Write(room.RoomName);
                    for (int i = 0; i < room.MyDevices.Count(); i++)
                    {
                        Console.Write(room.MyDevices[i].DeviceName);
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

        public bool DeleteRoom(Room room)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Room[])ctx.Cache[CacheKey]).ToList();
                    for (int i = 0; i < currentData.Count; i++)
                        if (room.RoomId == currentData.ElementAt(i).RoomId && room.RoomId == currentData.ElementAt(i).RoomId) //search for the matching name to delete
                        {
                            currentData.RemoveAt(i);
                        }
                    for (int i = 0; i < currentData.Count; i++)
                        System.Diagnostics.Debug.WriteLine(currentData.ElementAt(i).RoomId);  //this serves as a check to see if the item was deleted
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
