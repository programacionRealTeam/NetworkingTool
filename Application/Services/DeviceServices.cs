using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using Application.Response;
using Application.Helpers;

namespace Application.Services
{
    public class DeviceServices : IDeviceServices
    {
        public DeviceResponse ping(Device device)
        {
            //creo mi respuesta y mapeo al dispositivo
            DeviceResponse response= Mappers.mapperToResponse(device);

            try
            {
                using (Ping pingSender = new Ping())
                {
                    response.ip = device.ipAddress;
                    PingReply pingReply = pingSender.Send(device.ipAddress);
                    response.Status = pingReply.Status;
                    response.RoundtripTime = pingReply.RoundtripTime;
                    response.Timestamp = DateTime.Now;
                    
                }
            }
            catch (PingException)
            {
                response.Status = IPStatus.Unknown;
                response.RoundtripTime = -1; 
                                                 
            }

            return response;
        }


    }
}
