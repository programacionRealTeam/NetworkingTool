using Application.Models;
using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class Mappers
    {
        public static DeviceResponse mapperToResponse(Device device)
        {
            DeviceResponse response = new DeviceResponse();
            response.ip = device.ipAddress;
            response.name = device.name;
            response.prioridad = device.prioridad;
            response.category = device.category;
            return response;
        }

        public static Device mapperToDevice(DeviceRequest request)
        {
            Device device = new Device();
            device.name = request.name;
            device.ipAddress = request.ip;
            device.prioridad = request.prioridad;
            device.category = request.category;
            return device;
        }

    }
}
