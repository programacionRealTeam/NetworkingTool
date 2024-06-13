using Application.Interfaces;
using Application.Response;

using System.Net.NetworkInformation;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Querys
{
    public class DeviceOfflineQuerys : IDeviceOfflineQuery
    {
        private readonly MongoDBContext _context;

        public DeviceOfflineQuerys()
        {
            this._context = new MongoDBContext();
        }

        public List<DeviceOffline> GetAll()
        {
            var coleccion = _context.ObtenerColeccion("DevicesOffline");
            var bsonDevices = coleccion.Find(new BsonDocument()).ToList();
            return MapBsonToDevices(bsonDevices);
        }

        public List<DeviceOffline> GetAllByCategory(string category)
        {
            var coleccion = _context.ObtenerColeccion("DevicesOffline");
            var filter = Builders<BsonDocument>.Filter.Eq("category", category);
            var bsonDevices = coleccion.Find(filter).ToList();
            return MapBsonToDevices(bsonDevices);
        }

        public List<DeviceOffline> GetAllByPriority(string prioridad)
        {
            var coleccion = _context.ObtenerColeccion("DevicesOffline");
            var filter = Builders<BsonDocument>.Filter.Eq("prioridad", prioridad);
            var bsonDevices = coleccion.Find(filter).ToList();
            return MapBsonToDevices(bsonDevices);
        }

        public DeviceOffline GetByIp(string ip)
        {
            var coleccion = _context.ObtenerColeccion("DevicesOffline");
            var filter = Builders<BsonDocument>.Filter.Eq("ipAddress", ip);
            var bsonDevice = coleccion.Find(filter).FirstOrDefault();
            return MapBsonToDevice(bsonDevice);
        }

        public DeviceOffline GetByName(string name)
        {
            var coleccion = _context.ObtenerColeccion("DevicesOffline");
            var filter = Builders<BsonDocument>.Filter.Eq("name", name);
            var bsonDevice = coleccion.Find(filter).FirstOrDefault();
            return MapBsonToDevice(bsonDevice);
        }

        private List<DeviceOffline> MapBsonToDevices(List<BsonDocument> bsonDevices)
        {
            var devices = new List<DeviceOffline>();
            foreach (var bsonDevice in bsonDevices)
            {
                var device = new DeviceOffline
                {
                    name = bsonDevice["name"].AsString,
                    ip = bsonDevice["ip"].AsString,
                    category = bsonDevice["category"].AsString,
                    prioridad = bsonDevice["prioridad"].AsString,
                    Status = bsonDevice["Status"].AsString,
                    RoundtripTime = bsonDevice["RoundtripTime"].ToInt64(),
                    Timestamp = bsonDevice["Timestamp"].ToLocalTime(),
                };
                devices.Add(device);
            }
            return devices;
        }

        private DeviceOffline MapBsonToDevice(BsonDocument bsonDevice)
        {
            if (bsonDevice == null) return null;

            var device = new DeviceOffline
            {
                name = bsonDevice["name"].AsString,
                ip = bsonDevice["ip"].AsString,
                category = bsonDevice["category"].AsString,
                prioridad = bsonDevice["prioridad"].AsString,
                Status = bsonDevice["Status"].AsString,
                RoundtripTime = bsonDevice["RoundtripTime"].ToInt64(),
                Timestamp = bsonDevice["Timestamp"].ToLocalTime(),
            };
            return device;
        }

    }
}

