using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext()
        {
            // Configura tu conexión a MongoDB aquí
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("networkingTool"); // Reemplaza "MiBaseDeDatos" con el nombre de tu base de datos
        }

        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }


    }
}
