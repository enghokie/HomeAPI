using HomeAPI.Models;
using HomeAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HomeAPI.Controllers
{
    public class UserController : ApiController
    {

        private UserRepository userRepository;

        public UserController()
        {
            this.userRepository = new UserRepository();
        }

        public User[] Get()
        {
            return userRepository.GetAllUsers();
        }

        public HttpResponseMessage Patch()
        {
            Exception ex = this.userRepository.UpdatHouses();
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotModified, ex);
            if (ex.Message == "updated")
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);

            return response;
        }

        public HttpResponseMessage Post(User user)
        {
            Exception ex = this.userRepository.SaveUser(user);
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotAcceptable, ex);

            if (ex.Message == "none")
                response = Request.CreateResponse<User>(System.Net.HttpStatusCode.Created, user);
            
            return response;
        }

        public HttpResponseMessage Delete(User user)
        {
            Exception ex = this.userRepository.DeleteUser(user);
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotAcceptable, ex);

            if (ex.Message == "none")
                response = Request.CreateResponse<User>(System.Net.HttpStatusCode.OK, user);
            return response;
        }
    }
}
