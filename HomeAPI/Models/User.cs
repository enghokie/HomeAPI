using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeAPI.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<House> MyHouses;
    }
}
