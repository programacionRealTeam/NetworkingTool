using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DeviceOffline :IDevice
    {
        public string name { get; set; }
        public string ip { get; set; }
        public string category { get; set; }
        public string prioridad { get; set; }
        public string Status { get; set; }
        public long RoundtripTime { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
