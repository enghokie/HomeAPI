using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// House class model

namespace HomeAPI.Models
{
    public class House
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string UserName { get; set; }                // The username the house belongs to
        public List<Room> MyRooms = new List<Room>();       // List of rooms the house has
    }
}
