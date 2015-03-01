using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeAPI.Services;

namespace HomeAPI.Models
{
    public class Room : Controller
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int HouseId { get; set; }
        public Device[] MyDevices;
    }
}
