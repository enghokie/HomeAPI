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
    public class RoomController : ApiController
    {
        private RoomRepository roomRepository;

        public RoomController()
        {
            this.roomRepository = new RoomRepository();
        } 

        public Room[] Get()
        {
            return roomRepository.GetAllRooms();
        }

        public HttpResponseMessage Patch()
        {
            Exception ex = this.roomRepository.UpdatDevices();
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotModified, ex);
            if (ex.Message == "updated")
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);

            return response;
        }

        public HttpResponseMessage Post(Room room)
        {
            Exception ex = this.roomRepository.SaveRoom(room);
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotAcceptable, ex);

            if (ex.Message == "none")
                response = Request.CreateResponse<Room>(System.Net.HttpStatusCode.Created, room);
            
            return response;
        }

        public HttpResponseMessage Delete(Room room)
        {
            Exception ex = this.roomRepository.DeleteRoom(room);
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotAcceptable, ex);

            if (ex.Message == "none")
                response = Request.CreateResponse<Room>(System.Net.HttpStatusCode.OK, room);

            return response;
        }
    }
}
