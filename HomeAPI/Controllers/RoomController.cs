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

        public HttpResponseMessage Post(Room room)
        {
            this.roomRepository.SaveRoom(room);

            var response = Request.CreateResponse<Room>(System.Net.HttpStatusCode.Created, room);

            return response;
        }

        public HttpResponseMessage Delete(Room room)
        {
            this.roomRepository.DeleteRoom(room);

            var response = Request.CreateResponse<Room>(System.Net.HttpStatusCode.OK, room);

            return response;
        }
    }
}
