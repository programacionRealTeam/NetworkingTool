using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using Application.Response;
using Application.Helpers;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class DeviceServices : IDeviceServices
    {

        public DeviceResponse ping(Device device)
        {
            //creo mi respuesta y mapeo al dispositivo
            DeviceResponse response= Mappers.mapperToResponse(device);

            try
            {
                using (Ping pingSender = new Ping())
                {
                    response.ip = device.ipAddress;
                    PingReply pingReply = pingSender.Send(device.ipAddress);
                    response.Status = pingReply.Status.ToString();
                    response.RoundtripTime = pingReply.RoundtripTime;
                    response.Timestamp = DateTime.UtcNow;
                    
                }
            }
            catch (PingException)
            {
                response.Status = IPStatus.Unknown.ToString();
                response.RoundtripTime = -1; 
                                                 
            }

            return response;
        }

        public List<string> obtenerMAC(Device dispositivo)
        {
            List<string> macAddresses = new List<string>();

            //return MAC;

            var devices = new List<string>();
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "arp",
                    Arguments = "-a",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();

            // Leer la salida del comando arp -a
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // Buscar la línea que contiene la dirección IP del dispositivo
            string pattern = @"\b" + Regex.Escape(dispositivo.ipAddress) + @"\b\s+([a-fA-F0-9:-]+)\s";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(output);

            if (match.Success)
            {
                // Obtener la dirección MAC de la línea correspondiente
                string macAddress = match.Groups[1].Value;
                macAddresses.Add(macAddress);
            }

            return macAddresses;
        }

    }
}
