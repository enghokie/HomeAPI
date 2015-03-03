﻿using System;
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
                    MyDevices = new List<Device>()
                }
            };
        }

        public Exception UpdatDevices()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                var currentData = ((Room[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                bool found = false;
                bool match = false;
                bool noMatch = false;
                DeviceRepository deviceRepo = new DeviceRepository();
                List<Device> deviceList = new List<Device>();
                deviceList = deviceRepo.GetAllDevices().ToList();
                for (int i = 0; i < currentData.Count; i++)
                {
                    for (int j = 0; j < deviceList.Count; j++)
                    {
                        if (currentData.ElementAt(i).RoomId == deviceList[j].RoomId)
                        {
                            if (currentData.ElementAt(i).MyDevices.Count > 0)
                            {
                                for (int k = 0; k < currentData.ElementAt(i).MyDevices.Count; k++)
                                {
                                    if (currentData.ElementAt(i).MyDevices[k].DeviceId == deviceList[j].DeviceId)
                                    {
                                        found = true;
                                        currentData.ElementAt(i).MyDevices[k] = deviceList[j];
                                    }
                                }
                                if (!found)
                                    currentData.ElementAt(i).MyDevices.Add(deviceList[j]);
                            }
                            else
                            {
                                currentData.ElementAt(i).MyDevices.Add(deviceList[j]);
                            }
                        }

                        for (int k = 0; k < currentData.ElementAt(i).MyDevices.Count; k++)
                        {
                            if (currentData.ElementAt(i).MyDevices[k].DeviceId == deviceList[j].DeviceId)
                                match = true;
                            
                            if (!match && (k >= currentData.ElementAt(i).MyDevices.Count))
                            {
                                noMatch = true;
                                currentData.ElementAt(i).MyDevices[k] = null;
                            }
                        }
                    }
                }
                ctx.Cache[CacheKey] = currentData.ToArray();
                if (noMatch || found || !found)
                    return new Exception("updated");
            }
            return new Exception("No devices found for the rooms available");
        }

        public Exception SaveRoom(Room room)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Room[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                    bool added = false;
                    HouseRepository tempRepo = new HouseRepository();
                    List<House> houseList = new List<House>();
                    houseList = tempRepo.GetAllHouses().ToList();
                    RoomRepository tempRepo2 = new RoomRepository();
                    List<Room> roomList = new List<Room>();
                    roomList = tempRepo2.GetAllRooms().ToList();

                    for (int i = 0; i < roomList.Count; i++)
                    {
                        if (roomList[i].RoomId == room.RoomId)
                        {
                            roomList[i] = room;
                            ctx.Cache[CacheKey] = roomList.ToArray();
                            throw new Exception("Already have Room: " + room.RoomName +
                            " updating properties");
                        }
                    }

                    for (int i = 0; i < houseList.Count; i++)
                    {
                        if (houseList[i].HouseId == room.HouseId)
                        {
                            roomList.Add(room);
                            added = true;
                        }                                                           //add the new device
                    }

                    if (!added)
                        throw new Exception("Could not find House ID: " + room.HouseId);

                    ctx.Cache[CacheKey] = roomList.ToArray();                //recache the array
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ex;
                }
            }

            return new Exception("none");
        }

        public Exception DeleteRoom(Room room)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Room[])ctx.Cache[CacheKey]).ToList();
                    bool found = false;
                    for (int i = 0; i < currentData.Count; i++)
                    {
                        if (room.RoomId == currentData.ElementAt(i).RoomId) //search for the matching name to delete
                        {
                            currentData.RemoveAt(i);
                            found = true;
                        }
                    }
                    for (int i = 0; i < currentData.Count; i++)
                        System.Diagnostics.Debug.WriteLine(currentData.ElementAt(i).RoomId);  //this serves as a check to see if the item was deleted
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    if (!found)
                        throw new Exception("Could not find Room: " + room.RoomName);
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
