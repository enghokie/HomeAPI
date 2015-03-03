using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeAPI.Models
{
    public class House
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string UserName { get; set; }
        public List<Room> MyRooms;
    }
}
