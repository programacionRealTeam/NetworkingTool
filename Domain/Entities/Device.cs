using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Domain.Entities
{
    public class Device :IDevice
    {
        public string name { get; set; }
        public string ipAddress { get; set; }
        public string category { get; set; }
        public string prioridad { get; set; }
  
    }
}
