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
                    UserName = "Placeholder",
                    Password = "Placeholder",
                    MyHouses = new List<House>()
                }
            };
        }

        public Exception SaveUser(User user)
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
                            throw new Exception("Already have user: " + user.UserName);
                    }
                    
                    currentData.Add(user);
                    ctx.Cache[CacheKey] = currentData.ToArray();                //recache the array
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
                        if (user.UserName == currentData.ElementAt(i).UserName && user.UserName == currentData.ElementAt(i).UserName) //search for the matching name to delete
                        {
                            if (user.Password == currentData.ElementAt(i).Password && user.Password == currentData.ElementAt(i).Password)
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
