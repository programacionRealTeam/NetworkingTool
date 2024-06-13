using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDeviceOfflineCommand
    {
        Task Insert(Response.DeviceResponse device);
        Task Update(Response.DeviceResponse device);
        Task Delete(string name);
    }
}
