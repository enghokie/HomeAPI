using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Room class model

namespace HomeAPI.Models
{
    public class Room
    {
        public int RoomId { get; set; }             
        public string RoomName { get; set; }
        public int HouseId { get; set; }        // The ID of the house this room belongs to
        public List<Device> MyDevices = new List<Device>(); // A list of devices that belong to this room
    }
}
