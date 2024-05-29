using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDeviceServices
    {

        DeviceResponse Ping(Device device);
        void CreateDevice(DeviceRequest request);
        List<DeviceRequest> GetAllDevices();
        void UpdateDevice(DeviceRequest request);
        void DeleteDevice(string name, string ip);

    }
}
