using Application.Interfaces;
using Application.Response;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DeviceCommand : IDeviceCommand
    {
        private readonly MongoDBContext _context;

        public DeviceCommand()
        {
            this._context = new MongoDBContext();

        }

        public async Task Insert(Device device)
        {
            var coleccion = _context.ObtenerColeccion("Devices"); // Reemplaza "MiColeccion" con el nombre de tu colección

            // Crear un documento BSON
            var documento = new BsonDocument
                {
                    { "name",device.name },
                    { "ip", device.ipAddress },
                    { "category", device.category},
                    { "prioridad", device.prioridad}
                };

            // Insertar el documento en la colección
            await coleccion.InsertOneAsync(documento);
        }

        public async Task Update(Device device)
        {
            var coleccion = _context.ObtenerColeccion("Devices");

            var filter = Builders<BsonDocument>.Filter.Eq("name", device.name);

            var update = Builders<BsonDocument>.Update
                .Set("ip", device.ipAddress)
                .Set("category", device.category)
                .Set("prioridad", device.prioridad);

            await coleccion.UpdateOneAsync(filter, update);
        }

        public async Task Delete(string name)
        {
            var coleccion = _context.ObtenerColeccion("Devices");

            var filter = Builders<BsonDocument>.Filter.Eq("name", name);

            await coleccion.DeleteOneAsync(filter);
        }
    }
}
