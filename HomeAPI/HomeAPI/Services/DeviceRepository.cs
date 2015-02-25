using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAPI.Models;

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
                    Id = 0,
                    Name = "Placeholder"
                }
            };
        }

        public bool SaveDevice(Device device)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Device[])ctx.Cache[CacheKey]).ToList();
                    currentData.Add(device);
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