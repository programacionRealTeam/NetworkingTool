using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class CassandraContext
    {
        private static readonly Lazy<Cluster> cluster = new Lazy<Cluster>(() => Cluster.Builder()
            .AddContactPoint("127.0.0.1") //conexion mediante localhost
            .Build());

        private static ISession session;

        public static ISession GetSession()
        {
            if (session == null)
            {
                session = cluster.Value.Connect("networkingdb"); // aca se cambia para el nombre de la keyspace que se tiene en la maquina
            }
            return session;
        }
    }
}
