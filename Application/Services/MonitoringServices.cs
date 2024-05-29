using Application.Helpers;
using Application.Interfaces;
using Domain.Entities;
using Application.Models;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.IO;
using Domain.Interfaces;
using System.Threading;
using Application.Services;
using Cassandra;


namespace Application.Services
{
    public class MonitoringServices : IMonitoringServices
    {
        private readonly DeviceServices _deviceServices;

        public MonitoringServices()
        {
            _deviceServices = new DeviceServices();
        }

        public void InitRealTime(List<DeviceRequest> listRequest)
        {
            bool isRunning = true;

            Thread sweepThread = new Thread(() =>
            {
                while (isRunning)
                {
                    Sweep(listRequest);
                }
            });

            sweepThread.Start();
            sweepThread.Join();
        }

        public List<DeviceResponse> Sweep(List<DeviceRequest> listRequest)
        {
            List<DeviceResponse> devicesOffline = new List<DeviceResponse>();

            foreach (DeviceRequest request in listRequest)
            {
                var device = Mappers.mapperToDevice(request);
                var response = _deviceServices.Ping(device);

                if (response.Status != IPStatus.Success)
                {
                    devicesOffline.Add(response);
                    _deviceServices.CreateDevice(request); // Guardar en Cassandra
                }
            }

            return devicesOffline;
        }

        

        public string InitLogs()
        {
            // Implementación de logs aquí, por ahora solo devolvemos un mensaje.
            string mensaje = "Listado de dispositivos que tuvieron perdidas de conexion:";
            return mensaje;
        }

        public void CreateDevice(DeviceRequest device)
        {
            // Por ejemplo, utilizando la clase CassandraContext para interactuar con la base de datos
            using (ISession session = CassandraContext.GetSession())
            {
                // Ejemplo de código para insertar el dispositivo en una tabla de Cassandra [direcciones -> nombre de la tabla ]
                string query = $"INSERT INTO direcciones (name, ip, category, prioridad) VALUES (?, ?, ?, ?)";

                var preparedStatement = session.Prepare(query);
                var boundStatement = preparedStatement.Bind(device.name, device.ip, device.category, device.prioridad);

                session.Execute(boundStatement);
            }

            Console.WriteLine("Dispositivo guardado correctamente.");
        }


    }
}