using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAPI.Models;

// This class tells the controller how to process HTTP commands

namespace HomeAPI.Services
{
    public class UserRepository
    {
        private const string CacheKey = "UserStorage";

        public UserRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var users = new User[] { }; //Load the users from the back end data structure

                    ctx.Cache[CacheKey] = users;
                }
            }
        }

        public User[] GetAllUsers()     // Gets all the users from the cache
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (User[])ctx.Cache[CacheKey];
            }

            return new User[]
            {
            new User
                {
                    UserName = "Placeholder",
                    Password = "Placeholder",
                    MyHouses = new List<House>()
                }
            };
        }

        public Exception UpdatHouses()  // Updates the houses from the house list based on cache
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                var currentData = ((User[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                bool found = false;
                bool match = false;
                bool noMatch = false;
                HouseRepository houseRepo = new HouseRepository();
                List<House> houseList = new List<House>();
                houseList = houseRepo.GetAllHouses().ToList();
                for (int i = 0; i < currentData.Count; i++)
                {
                    for (int j = 0; j < houseList.Count; j++)
                    {
                        if (currentData.ElementAt(i).UserName == houseList[j].UserName)
                        {
                            if (currentData.ElementAt(i).MyHouses.Count > 0)
                            {
                                for (int k = 0; k < currentData.ElementAt(i).MyHouses.Count; k++)
                                {
                                    if (currentData.ElementAt(i).MyHouses[k].HouseId == houseList[j].HouseId)       // If the house exist in cache, update list
                                    {
                                        found = true;
                                        currentData.ElementAt(i).MyHouses[k] = houseList[j];
                                    }
                                }
                                if (!found)
                                    currentData.ElementAt(i).MyHouses.Add(houseList[j]); // If it doesn't exist in the user house list, add it
                            }
                            else
                            {
                                currentData.ElementAt(i).MyHouses.Add(houseList[j]);
                            }
                        }

                        for (int k = 0; k < currentData.ElementAt(i).MyHouses.Count; k++)
                        {
                            if (currentData.ElementAt(i).MyHouses[k].HouseId == houseList[j].HouseId)
                                match = true;

                            if (!match && (k >= currentData.ElementAt(i).MyHouses.Count))
                            {
                                noMatch = true;
                                currentData.ElementAt(i).MyHouses[k] = null;    // If it was removed from the cache, remove it from the list
                            }
                        }
                    }
                }
                ctx.Cache[CacheKey] = currentData.ToArray();
                if (noMatch || found || !found)
                    return new Exception("updated");
            }
            return new Exception("No houses found for the users available");
        }

        public Exception SaveUser(User user)    // Creates and saves a new user
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((User[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                    UserRepository tempRepo = new UserRepository();
                    List<User> userList = new List<User>();
                    userList = tempRepo.GetAllUsers().ToList();

                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (userList[i].UserName == user.UserName)
                        {
                            throw new Exception("Already have User: " + user.UserName); // Check if the user already exist
                        }
                    }
                    
                    userList.Add(user);
                    ctx.Cache[CacheKey] = userList.ToArray();                //recache the array
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ex;
                }
            }

            return new Exception("none");
        }

        public Exception DeleteUser(User user)
        {
            var ctx = HttpContext.Current;
            bool found = false;
            if (ctx != null)
            {
                try
                {
                    var currentData = ((User[])ctx.Cache[CacheKey]).ToList();
                    for (int i = 0; i < currentData.Count; i++)
                    {
                        if (user.UserName == currentData.ElementAt(i).UserName) //search for the matching name to delete
                        {
                            if (user.Password == currentData.ElementAt(i).Password)
                            {
                                currentData.RemoveAt(i);
                                found = true;
                            }
                            else
                                throw new Exception("Invalid password for User: " + user.UserName);
                        }
                    }
                    for (int i = 0; i < currentData.Count; i++)
                        System.Diagnostics.Debug.WriteLine(currentData.ElementAt(i).UserName);  //this serves as a check to see if the item was deleted
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    if (!found)
                        throw new Exception("Could not find User: " + user.UserName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ex;
                }
            }

            return new Exception("none");;
        }
    } 
}
