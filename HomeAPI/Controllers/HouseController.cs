﻿using HomeAPI.Models;
using HomeAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HomeAPI.Controllers
{
    public class HouseController : ApiController
    {
       private HouseRepository houseRepository;

        public HouseController()
        {
            this.houseRepository = new HouseRepository();
        } 

        public House[] Get()                        // HTTP GET - gets information about house
        {
            return houseRepository.GetAllHouses();
        }

        public HttpResponseMessage Patch()                      // HTTP PATCH - updates information about the house
        {
            Exception ex = this.houseRepository.UpdatRooms();
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotModified, ex);
            if (ex.Message == "updated")
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);

            return response;
        }

        public HttpResponseMessage Post(House house)                // HTTP POST - posts a new house
        {
            Exception ex = this.houseRepository.SaveHouse(house);
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotAcceptable, ex);

            if (ex.Message == "none")
                response = Request.CreateResponse<House>(System.Net.HttpStatusCode.Created, house);
            
            return response;
        }

        public HttpResponseMessage Delete(House house)              // HTTP DELETE - deletes a house
        {
            Exception ex = this.houseRepository.DeleteHouse(house);
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotAcceptable, ex);

            if (ex.Message == "none")
                response = Request.CreateResponse<House>(System.Net.HttpStatusCode.OK, house);

            return response;
        }
    }
}
