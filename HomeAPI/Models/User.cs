using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// User class model

namespace HomeAPI.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<House> MyHouses = new List<House>(); // List of houses the user has
    }
}
