using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeAPI.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int HouseId { get; set; }
        public List<Device> MyDevices;
    }
}
