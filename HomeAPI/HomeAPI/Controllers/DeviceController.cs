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
    public class DeviceController : ApiController
    {

        private DeviceRepository deviceRepository;

        public DeviceController()
        {
            this.deviceRepository = new DeviceRepository();
        } 

        public Device[] Get()
        {
            return deviceRepository.GetAllDevices();
        }

        public HttpResponseMessage Post(Device device)
        {
            this.deviceRepository.SaveDevice(device);

            var response = Request.CreateResponse<Device>(System.Net.HttpStatusCode.Created, device);

            return response;
        }
    }
}
