using Application.Interfaces;
using Application.Models;
using Application.Response;
using Application.Services;
using Infrastructure;
using Infrastructure.Querys;
using Infrastructure.Repo;
using System.Net.NetworkInformation;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RealTime
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //lista de dispositivos importados
            List<DeviceRequest> deviceList = ImportDevice(@"..\..\..\Infrastructure\Persistence\importacionIP.txt");
            
            //"Controladores"
    
            DeviceOfflineCommand offlineCommand = new DeviceOfflineCommand();
            DeviceCommand deviceCommand = new DeviceCommand();
            DeviceQuerys deviceQuery = new DeviceQuerys();
            DeviceOfflineQuerys deviceOfflineQuerys = new DeviceOfflineQuerys();


            //inicializacion de servicio de monitoreo
            MonitoringServices service = new MonitoringServices(deviceCommand, offlineCommand,deviceQuery, deviceOfflineQuerys);
            service.ImportList(deviceList); // inicializo servicio de monitoreo
            
            //transparencia de inicio del sistema
            Console.WriteLine("Inicializando servicios...");

            Transparencia("Inicializando servicio de monitoreo de tiempo real...",3000);

            //inicializo hilo de para la busqueda de dispositivos que vuelven a estar online
            Thread offllineSearch = new Thread(() =>
            {
                //Bucle continuo de impresion de dispositivos offline
                while (service.isRunning())
                {
                    EncabezadoTabla();
                    foreach (DeviceResponse device in service.getOfflineDevices())
                    {
                        Console.WriteLine("| {0,-20} | {1,-15} | {2,-15} | {3,-10} | {4,-26} | {5,-8} | {6,-25} |",
                        device.name, device.ip, device.category, device.prioridad, device.Status, device.RoundtripTime, device.Timestamp);
                    }
                    PieTabla();
                }
            });

            offllineSearch.Start();

        }



        //FUNCIONES
        
       //importacion de dispositivos desde txt
        static List<DeviceRequest> ImportDevice(string filePath)
        {
            List<DeviceRequest> deviceList = new List<DeviceRequest>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(';');
                    if (data.Length == 4)
                    {
                        var device = new DeviceRequest
                        {
                            name = data[0],
                            ip = data[1],
                            category = data[2],
                            prioridad = data[3]
                        };
                        deviceList.Add(device);
                    }
                }
            }

            return deviceList;
        }



        static void Transparencia(string mensaje, int duracionMilisegundos)
        {
            int progreso = 0;
            int paso = 5; // Incremento de progreso por cada paso (5%)

            while (progreso < 100)
            {
                progreso += paso;
                MostrarBarraProgreso(mensaje,progreso);
                Thread.Sleep(duracionMilisegundos / (100 / paso));
            }

            // Limpia la consola después de completar la barra de progreso
            Console.Clear();
        }

        static void MostrarBarraProgreso(string mensaje, int porcentaje)
        {
            Console.Write($"\r{mensaje}: [");

            int caracteres = porcentaje / 2; // Cada 2% es un caracter de barra '[#####     ]'
            for (int i = 0; i < caracteres; i++)
            {
                Console.Write("#");
            }

            for (int i = caracteres; i < 50; i++)
            {
                Console.Write("-");
            }

            Console.Write("]" );
        }

        static void EncabezadoTabla()
        {
            // Imprimir encabezado de tabla
            Console.WriteLine("+----------------------+-----------------+-----------------+------------+----------------------------+----------+---------------------------+");

            Console.WriteLine("| {0,-20} | {1,-15} | {2,-15} | {3,-10} | {4,-26} | {5,-8} | {6,-25} |",
                "Nombre Dispositivo", "Direccion IP", "Categoría", "Prioridad", "Estado", "Time ms", "Ultima prueba de conexion");

            // Imprimir línea de separación
            Console.WriteLine("+----------------------+-----------------+-----------------+------------+----------------------------+----------+---------------------------+");

        }

        static void PieTabla()
        {

            // Imprimir línea de separación
           Console.WriteLine("+----------------------+-----------------+-----------------+------------+----------------------------+----------+---------------------------+");
           Transparencia("Procesando direcciones ip", 3000);

        }
    }
}
