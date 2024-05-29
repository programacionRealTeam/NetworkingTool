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
using Cassandra;
using Application.Models;



namespace Application.Services
{
    public class DeviceServices : IDeviceServices
    {
        private readonly ISession _session;

        public DeviceServices()
        {
            _session = CassandraContext.GetSession();
        }

        public DeviceResponse Ping(Device device)
        {
            DeviceResponse response = Mappers.mapperToResponse(device);

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

        public void CreateDevice(DeviceRequest request)
        {
            Device device = Mappers.mapperToDevice(request);
            var query = "INSERT INTO devices (name, ip, category, prioridad) VALUES (?, ?, ?, ?)";
            var statement = _session.Prepare(query).Bind(device.name, device.ipAddress, device.category, device.prioridad);
            _session.Execute(statement);
        }

        public List<DeviceRequest> GetAllDevices()
        {
            var devices = new List<DeviceRequest>();
            var query = "SELECT * FROM devices";
            var rs = _session.Execute(query);

            foreach (var row in rs)
            {
                var device = new DeviceRequest
                {
                    name = row.GetValue<string>("name"),
                    ip = row.GetValue<string>("ip"),
                    category = row.GetValue<string>("category"),
                    prioridad = row.GetValue<string>("prioridad")
                };
                devices.Add(device);
            }

            return devices;
        }

        public void UpdateDevice(DeviceRequest request)
        {
            var query = "UPDATE devices SET category = ?, prioridad = ? WHERE name = ? AND ip = ?";
            var statement = _session.Prepare(query).Bind(request.category, request.prioridad, request.name, request.ip);
            _session.Execute(statement);
        }

        public void DeleteDevice(string name, string ip)
        {
            var query = "DELETE FROM devices WHERE name = ? AND ip = ?";
            var statement = _session.Prepare(query).Bind(name, ip);
            _session.Execute(statement);
        }
    }
}