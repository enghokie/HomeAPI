using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAPI.Models;

//This class serves as the data storage space. It is just a list of Device items that are regenerated from the cache

namespace HomeAPI.Services
{
    public class DeviceRepository
    {
        private const string CacheKey = "DeviceStore";

        public DeviceRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var devices = new Device[]{}; //Load the devices from the back end data structure??

            ctx.Cache[CacheKey] = devices;
                }
            }
        }

        public Device[] GetAllDevices()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (Device[])ctx.Cache[CacheKey];
            }

            return new Device[]
            {
            new Device
                {
                    DeviceId = 0,
                    DeviceName = "Placeholder",
                    RoomId = 0
                }
            };
        }

        public Exception SaveDevice(Device device)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Device[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                    bool added = false;
                    var roomData = ((Room[])ctx.Cache["RoomStore"]).ToList();
                    DeviceRepository tempRepo2 = new DeviceRepository();
                    List<Device> deviceList = new List<Device>();
                    deviceList = tempRepo2.GetAllDevices().ToList();

                    for (int i = 0; i < deviceList.Count; i++)
                    {
                        if (deviceList[i].DeviceId == device.DeviceId)
                        {
                            deviceList[i] = device;
                            ctx.Cache[CacheKey] = deviceList.ToArray();
                            throw new Exception("Already have Device: " + device.DeviceName +
                            " updating properties");
                        }
                    }

                    for (int i = 0; i < roomData.Count; i++)
                    {
                        if (roomData.ElementAt(i).RoomId == device.RoomId)
                        {
                            deviceList.Add(device);
                            added = true;
                        }                                                           //add the new device
                    }

                    if (!added)
                        throw new Exception("Could not find Room ID: " + device.RoomId);

                    ctx.Cache[CacheKey] = deviceList.ToArray();                //recache the array
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ex;
                }
            }

            return new Exception("none");
        }

        public Exception DeleteDevice(Device device)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Device[])ctx.Cache[CacheKey]).ToList();
                    bool found = false;
                    for (int i = 0; i < currentData.Count; i++)
                    {
                        if (device.DeviceId == currentData.ElementAt(i).DeviceId)
                        {//search for the matching device to delete
                            currentData.RemoveAt(i);
                            found = true;
                        }
                    }
                    for (int i = 0; i < currentData.Count; i++)
                        System.Diagnostics.Debug.WriteLine(currentData.ElementAt(i).DeviceId);  //this serves as a check to see if the item was deleted
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    if (!found)
                        throw new Exception("Could not find Device: " + device.DeviceName);
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