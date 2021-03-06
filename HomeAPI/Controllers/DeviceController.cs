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
    public class DeviceController : ApiController
    {

        private DeviceRepository deviceRepository;

        public DeviceController()
        {
            this.deviceRepository = new DeviceRepository();
        } 

        public Device[] Get()                           // HTTP GET - gets information about the device
        {
            return deviceRepository.GetAllDevices();
        }

        public HttpResponseMessage Post(Device device)              // HTTP POST - posts a new device
        {
            Exception ex = this.deviceRepository.SaveDevice(device);
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotAcceptable, ex);

            if (ex.Message == "none")
                response = Request.CreateResponse<Device>(System.Net.HttpStatusCode.Created, device);
           
            return response;
        }

        public HttpResponseMessage Delete(Device device)            // HTTP DELETE - deltes a device
        {
            Exception ex = this.deviceRepository.DeleteDevice(device);
            var response = Request.CreateErrorResponse(System.Net.HttpStatusCode.NotAcceptable, ex);

            if (ex.Message == "none")
                response = Request.CreateResponse<Device>(System.Net.HttpStatusCode.OK, device);

            return response;
        }
    }
}
