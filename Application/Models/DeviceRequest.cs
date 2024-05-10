using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class DeviceRequest :IDevice
    {
        public string name { get; set; }
        public string ip { get; set; }
        public string category { get; set; }
        public string prioridad { get; set; }
    }
}
