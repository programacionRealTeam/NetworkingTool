using Application.Interfaces;
using Application.Response;
using Domain.Interfaces;
using Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
    public class DeviceOfflineCommand : IDeviceOfflineCommand
    {
        private readonly MongoDBContext _context;

        public DeviceOfflineCommand()
        {
            this._context = new MongoDBContext();

        }

        public async Task Insert(DeviceResponse response)
        {
            var coleccion = _context.ObtenerColeccion("DevicesOffline"); // Reemplaza "MiColeccion" con el nombre de tu colección

            // Crear un documento BSON
            var documento = new BsonDocument
                {
                    { "name", response.name },
                    { "ip", response.ip },
                    { "category", response.category},
                    { "prioridad", response.prioridad},
                    { "Status", response.Status},
                    { "RoundtripTime", response.RoundtripTime},
                    { "Timestamp", response.Timestamp}
                };

            // Insertar el documento en la colección
                await coleccion.InsertOneAsync(documento);
        }

        public async Task Update(DeviceResponse response)
        {
            var coleccion = _context.ObtenerColeccion("DevicesOffline");

            var filter = Builders<BsonDocument>.Filter.Eq("name", response.name);

            var update = Builders<BsonDocument>.Update
                .Set("ip", response.ip)
                .Set("category", response.category)
                .Set("prioridad", response.prioridad)
                .Set("Status", response.Status)
                .Set("RoundtripTime", response.RoundtripTime)
                .Set("Timestamp", response.Timestamp);

            await coleccion.UpdateOneAsync(filter, update);
        }

        public async Task Delete(string name)
        {
            var coleccion = _context.ObtenerColeccion("DevicesOffline");

            var filter = Builders<BsonDocument>.Filter.Eq("name", name);

            await coleccion.DeleteOneAsync(filter);
        }

    }
}
