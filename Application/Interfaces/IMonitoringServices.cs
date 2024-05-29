using Application.Helpers;
using Application.Models;
using Application.Response;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMonitoringServices
    {
        void InitRealTime(List<DeviceRequest> listRequest);
        List<DeviceResponse> Sweep(List<DeviceRequest> listRequest);
        string InitLogs();
        void CreateDevice(DeviceRequest device);
    }
}
