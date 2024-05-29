using Application.Models;
using Application.Services;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Application.Interfaces;
using System.Linq;//revisar

namespace RealTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Bienvenido al sistema de monitoreo en tiempo real");
                Console.WriteLine("1. Cargar datos");
                Console.WriteLine("2. Iniciar monitoreo en tiempo real");
                Console.WriteLine("3. Salir");

                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        CargarDatos();
                        break;
                    case "2":
                       // IniciarMonitoreo();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                        break;
                }
            }
        }

        static void CargarDatos()
        {
            Console.WriteLine("Ingrese los detalles del dispositivo:");

            Console.Write("Nombre del dispositivo: ");
            string nombre = Console.ReadLine();

            Console.Write("Dirección IP del dispositivo: ");
            string ip = Console.ReadLine();

            Console.Write("Categoría del dispositivo: ");
            string categoria = Console.ReadLine();

            Console.Write("Prioridad del dispositivo: ");
            string prioridad = Console.ReadLine();

            // Crear un objeto DeviceRequest con los datos ingresados por el usuario
            DeviceRequest dispositivo = new DeviceRequest
            {
                name = nombre,
                ip = ip,
                category = categoria,
                prioridad = prioridad
            };

            // Guardar el dispositivo en la base de datos
            GuardarDispositivo(dispositivo);
        }

        static void GuardarDispositivo(DeviceRequest dispositivo)
        {
            // Lógica para guardar el dispositivo en Cassandra
            MonitoringServices service = new MonitoringServices();
            service.CreateDevice(dispositivo);

            Console.WriteLine("Dispositivo guardado correctamente.");
        }

        /*static void IniciarMonitoreo()
        {
            MonitoringServices service = new MonitoringServices();

            // Recuperar dispositivos desde Cassandra
            List<DeviceRequest> deviceList = service.GetAllDevices();

            Thread initThread = new Thread(() =>
            {
                service.InitRealTime(deviceList);
            });

            initThread.Start();

            Console.WriteLine("Inicializando servicios...");
            Thread.Sleep(1000);
            Transparencia("Inicializando servicio de monitoreo de tiempo real", 10000);

            while (true)
            {
                var offlineDevices = service.Sweep(deviceList);

                EncabezadoTabla();
                foreach (var device in offlineDevices)
                {
                    Console.WriteLine("| {0,-20} | {1,-15} | {2,-15} | {3,-10} | {4,-26} | {5,-8} | {6,-25} |",
                        device.name, device.ip, device.category, device.prioridad, device.Status, device.RoundtripTime, device.Timestamp);
                }
                PieTabla();
            }
        }
        */
        static void Transparencia(string mensaje, int duracionMilisegundos)
        {
            int progreso = 0;
            int paso = 5;

            while (progreso < 100)
            {
                progreso += paso;
                MostrarBarraProgreso(mensaje, progreso);
                Thread.Sleep(duracionMilisegundos / (100 / paso));
            }

            Console.Clear();
        }

        static void MostrarBarraProgreso(string mensaje, int porcentaje)
        {
            Console.Write($"\r{mensaje}: [");

            int caracteres = porcentaje / 2;
            for (int i = 0; i < caracteres; i++)
            {
                Console.Write("#");
            }

            for (int i = caracteres; i < 50; i++)
            {
                Console.Write("-");
            }

            Console.Write("]");
        }

        static void EncabezadoTabla()
        {
            Console.WriteLine("+----------------------+-----------------+-----------------+------------+----------------------------+----------+---------------------------+");
            Console.WriteLine("| {0,-20} | {1,-15} | {2,-15} | {3,-10} | {4,-26} | {5,-8} | {6,-25} |",
                "Nombre Dispositivo", "Direccion IP", "Categoría", "Prioridad", "Estado", "Time ms", "Ultima prueba de conexion");
            Console.WriteLine("+----------------------+-----------------+-----------------+------------+----------------------------+----------+---------------------------+");
        }

        static void PieTabla()
        {
            Console.WriteLine("+----------------------+-----------------+-----------------+------------+----------------------------+----------+---------------------------+");
            Transparencia("Procesando direcciones ip", 3000);
        }
    }
}
       

