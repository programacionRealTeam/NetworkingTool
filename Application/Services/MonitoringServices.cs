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

namespace Application.Services
{
    public class MonitoringServices : IMonitoringServices
    {

        public void init(List<DeviceRequest> listRequest)
        {
            initRealTime(listRequest);
        }


        //inicializacion de monitoreo de tiempo real
        public void initRealTime(List<DeviceRequest> listRequest)
        {
            List<DeviceResponse> devicesOffline = new List<DeviceResponse>();

            // Ruta del archivo de texto dentro del proyecto
            string filePath = @"..\..\..\Infrastructure\Repo\responsesDevices.txt";
            bool isRunning = true;


            //inicializo hilo de barrido de dispositivos offline
            Thread sweepThread = new Thread(() =>
            {
                while (isRunning)
                {
                  devicesOffline = sweep(listRequest); //metodo de barrido
                }
             
            });

            sweepThread.Start();
            

            //inicializo hilo de escritura de la lista de devicesOffline
            Thread writerThread = new Thread(() =>
            {
            
                while (isRunning)
                {
                        // Escribir la lista de DeviceResponse en el archivo txt
                        lock (filePath)
                        {
                            // Escribir la lista de DeviceResponse en un nuevo archivo txt
                            using (StreamWriter writer = new StreamWriter(filePath))
                            {
                            
                                foreach (var response in devicesOffline)
                                {
                                   writer.WriteLine($"{response.name};{response.ip};{response.category};{response.prioridad};{response.Status};{response.RoundtripTime};{response.Timestamp}");
                                }
                            }
                        }
                        Thread.Sleep(2000);//tiempo de espera para que no se quiera acceder rapido al txt
                }
            });

            writerThread.Start();
            
            sweepThread.Join();
            writerThread.Join();
        }


        public string initLogs()
        {
           //A DESARROLLAR ETAPA DE ALMACENAMIENTO Y CONTEO DE TIEMPO MUERTO DE DISPOSITIVO
            string mensaje = "Listado de dispositivos que tuvieron perdidas de conexion:";
            return mensaje;
        }


        //ALGORITMO DE BARRIDO DE DISPOSITIVOS
        public List<DeviceResponse> sweep(List<DeviceRequest> listRequest)
        {

            //inicializo el servicio de dispositivos
            DeviceServices deviceServices = new DeviceServices();

            //creo lista de dispositivos offline que voy a ir actualizando
            List<DeviceResponse> devices= new List<DeviceResponse>();


            //recorro los dispositivos de la listRequest
            foreach (DeviceRequest device in listRequest)
            {
                // mapeo a device y lo dejo en memoria
                Device devInMemory = Mappers.mapperToDevice(device);

                //hago ping y devuelvo un device response con datos del ping 
                DeviceResponse response = deviceServices.ping(devInMemory);

                //si el estado no es sucess entonces lo agrego a la lsita de dispositivos con problemas
                if (response.Status != IPStatus.Success)
                {
                    devices.Add(response);
                }
            }

            //retorna los dispositivos con problemas
            return devices;

        }

    }
}
