using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeAPI.Models
{
    public class House : Controller
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string UserName { get; set; }
        public Room[] MyRooms;
    }
}
