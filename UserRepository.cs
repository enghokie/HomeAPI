using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAPI.Models;

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
                    var users = new User[] { }; //Load the users from the back end data structure??

                    ctx.Cache[CacheKey] = users;
                }
            }
        }

        public User[] GetAllUsers()
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
                    Name = "Placeholder",
                    Password = "Placeholder"
                }
            };
        }

        public bool SaveUser(User user)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((User[])ctx.Cache[CacheKey]).ToList(); //get a list of the current data
                    currentData.Add(user);                                    //add the new user
                    ctx.Cache[CacheKey] = currentData.ToArray();                //recache the array

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

        public bool DeleteUser(User user)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((User[])ctx.Cache[CacheKey]).ToList();
                    for (int i = 0; i < currentData.Count; i++)
                        if (user.Name == currentData.ElementAt(i).Name && user.Name == currentData.ElementAt(i).Name) //search for the matching name to delete
                        {
                            if (user.Password == currentData.ElementAt(i).Password && user.Password == currentData.ElementAt(i).Password)
                                currentData.RemoveAt(i);
                        }
                    for (int i = 0; i < currentData.Count; i++)
                        System.Diagnostics.Debug.WriteLine(currentData.ElementAt(i).Name);  //this serves as a check to see if the item was deleted
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
