using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
    public class DeviceRepo
    {
        public List<DeviceResponse> devices = new List<DeviceResponse>();

        public DeviceRepo(List<DeviceResponse> devices)
        {
            this.devices = devices;
        }
    }
}
