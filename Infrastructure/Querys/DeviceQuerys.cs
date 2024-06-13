using Application.Interfaces;
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
    public class DeviceQuerys : IDeviceQuery
    {
        private readonly MongoDBContext _context;

        public DeviceQuerys()
        {
            this._context = new MongoDBContext();
        }

        public List<Device> GetAll()
        {
            var coleccion = _context.ObtenerColeccion("Devices");
            var bsonDevices = coleccion.Find(new BsonDocument()).ToList();
            return MapBsonToDevices(bsonDevices);
        }

        public List<Device> GetAllByCategory(string category)
        {
            var coleccion = _context.ObtenerColeccion("Devices");
            var filter = Builders<BsonDocument>.Filter.Eq("category", category);
            var bsonDevices = coleccion.Find(filter).ToList();
            return MapBsonToDevices(bsonDevices);
        }

        public List<Device> GetAllByPriority(string prioridad)
        {
            var coleccion = _context.ObtenerColeccion("Devices");
            var filter = Builders<BsonDocument>.Filter.Eq("prioridad", prioridad);
            var bsonDevices = coleccion.Find(filter).ToList();
            return MapBsonToDevices(bsonDevices);
        }

        public Device GetByIp(string ip)
        {
            var coleccion = _context.ObtenerColeccion("Devices");
            var filter = Builders<BsonDocument>.Filter.Eq("ipAddress", ip);
            var bsonDevice = coleccion.Find(filter).FirstOrDefault();
            return MapBsonToDevice(bsonDevice);
        }

        public Device GetByName(string name)
        {
            var coleccion = _context.ObtenerColeccion("Devices");
            var filter = Builders<BsonDocument>.Filter.Eq("name", name);
            var bsonDevice = coleccion.Find(filter).FirstOrDefault();
            return MapBsonToDevice(bsonDevice);
        }

        public List<Device> MapBsonToDevices(List<BsonDocument> bsonDevices)
        {
            var devices = new List<Device>();
            foreach (var bsonDevice in bsonDevices)
            {
                var device = new Device
                {
                    name = bsonDevice["name"].AsString,
                    ipAddress = bsonDevice["ipAddress"].AsString,
                    category = bsonDevice["category"].AsString,
                    prioridad = bsonDevice["prioridad"].AsString
                };
                devices.Add(device);
            }
            return devices;
        }

        public Device MapBsonToDevice(BsonDocument bsonDevice)
        {
            if (bsonDevice == null) return null;

            var device = new Device
            {
                name = bsonDevice["name"].AsString,
                ipAddress = bsonDevice["ip"].AsString,
                category = bsonDevice["category"].AsString,
                prioridad = bsonDevice["prioridad"].AsString
            };
            return device;
        }

    }
}