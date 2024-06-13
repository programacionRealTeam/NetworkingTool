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
using System.Runtime.InteropServices.ComTypes;

namespace Application.Services
{
    public class MonitoringServices : IMonitoringServices
    {

        private readonly IDeviceCommand _deviceCommand;
        private readonly IDeviceOfflineCommand _offlineCommand;
        private readonly IDeviceQuery _deviceQuery;
        private readonly IDeviceOfflineQuery _deviceOfflineQuery;
        public static bool flagService = false;

        public MonitoringServices(IDeviceCommand deviceCommand, IDeviceOfflineCommand offlineCommand, IDeviceQuery deviceQuery, IDeviceOfflineQuery deviceOfflineQuery)
        {
            _deviceQuery = deviceQuery;
            _deviceOfflineQuery = deviceOfflineQuery;
            _deviceCommand = deviceCommand;
            _offlineCommand = offlineCommand;

        }


        public void ImportList(List<DeviceRequest> listReuqest)
        {
            List<Device> devices = new List<Device>();
            foreach (DeviceRequest deviceRequest in listReuqest)
            {
                Device device = Mappers.mapperToDevice(deviceRequest);

                if (_deviceQuery.GetByName(device.name) != null)
                {
                    _deviceCommand.Update(device);
                    devices.Add(device);
                }
                else
                {
                    _deviceCommand.Insert(device);
                    devices.Add(device);
                }
            }

            //inicia servicio de monitoreo con lista device 
           scan(devices);

        }

        public void scan(List<Device> listDevices)
        {
            List<Device> hightPriority = new List<Device>();
            List<Device> mediumPriority = new List<Device>();
            List<Device> lowPriority = new List<Device>();

            foreach (Device dev in listDevices)
            {
                switch (dev.prioridad)
                {
                    case "Alta":
                        hightPriority.Add(dev);
                        break;

                    case "Media":
                        mediumPriority.Add(dev);
                        break;

                    case "Baja":
                        lowPriority.Add(dev);
                        break;

                    default:
                        lowPriority.Add(dev);
                        break;
                }
            }
            Init(hightPriority,mediumPriority,lowPriority);
        }

        //inicializacion de monitoreo de tiempo real
        public void Init(List<Device> hightPriority, List<Device> mediumPriority, List<Device> lowPriority)
        {
            DeviceServices deviceServices = new DeviceServices();
            flagService = true;

            //inicializo hilo de para la busqueda de dispositivos hightPriority
            Thread priorityHigth = new Thread(() =>
            {
                while (true)
                {
                    foreach (Device dev in hightPriority)
                    {
                        DeviceResponse response = deviceServices.ping(dev);

                        if (response.Status != "Success")
                        {
                            if (_deviceOfflineQuery.GetByName(response.name) != null)
                            {
                                _offlineCommand.Update(response);
                            }
                            else
                            {
                                _offlineCommand.Insert(response);
                            }
                        }
                    }
                    Thread.Sleep(500);
                }
               
            });

            //inicializo hilo de para la busqueda de dispositivos mediumPriority
            Thread priorityMedium = new Thread(() =>
            {
                while (true)
                {
                    foreach (Device dev in mediumPriority)
                    {
                        DeviceResponse response = deviceServices.ping(dev);

                        if (response.Status != "Success")
                        {
                            if (_deviceOfflineQuery.GetByName(response.name) != null)
                            {
                                _offlineCommand.Update(response);
                            }
                            else
                            {
                                _offlineCommand.Insert(response);
                            }
                        }
                        Thread.Sleep(500);
                    }

                    Thread.Sleep(2000);
                }
               
            });

            //inicializo hilo de para la busqueda de dispositivos lowPriority
            Thread priorityLow = new Thread(() =>
            {
                while (true)
                {
                    foreach (Device dev in lowPriority)
                    {
                        DeviceResponse response = deviceServices.ping(dev);

                        if (response.Status != "Success")
                        {
                            if (_deviceOfflineQuery.GetByName(response.name) != null)
                            {
                                _offlineCommand.Update(response);
                            }
                            else
                            {
                                _offlineCommand.Insert(response);
                            }
                        }
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(4000);
                }
                
            });


            //inicializo hilo de para la busqueda de dispositivos que vuelven a estar online
            Thread onlineSearch = new Thread(() =>
            {
                List<DeviceOffline> deviceOffline = new List<DeviceOffline>();
                Device device = new Device();

                while (true)
                {
                    deviceOffline = _deviceOfflineQuery.GetAll(); //obtengo todos los dispositivos offline de la db

                    foreach (DeviceOffline dev in deviceOffline )
                    {
                        device.name = dev.name;
                        device.ipAddress = dev.ip;
                        device.prioridad = dev.prioridad;
                        device.category = dev.category;
                         
                        DeviceResponse response = deviceServices.ping(device);
                        if (response.Status == "Success")
                        {
                            _offlineCommand.Delete(response.name);
                        }
                    }
                }
            });

            priorityHigth.Start();
            priorityMedium.Start();
            priorityLow.Start();
            onlineSearch.Start();

        }

        public bool isRunning()
        {
            return flagService;
        }

        public string initLogs()
        {
            //A DESARROLLAR ETAPA DE ALMACENAMIENTO Y CONTEO DE TIEMPO MUERTO DE DISPOSITIVO
            string mensaje = "Listado de dispositivos que tuvieron perdidas de conexion:";
            return mensaje;
        }

        public List<DeviceResponse> getOfflineDevices()
        {
            List<DeviceResponse> offlineDevices = new List<DeviceResponse>();

            foreach (DeviceOffline device in _deviceOfflineQuery.GetAll())
            {
                offlineDevices.Add(Mappers.mapperOfflineToResponse(device));
            }

           return offlineDevices;

        }

    }
}
