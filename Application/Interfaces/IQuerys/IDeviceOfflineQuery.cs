using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDeviceOfflineQuery
    {
        DeviceOffline GetByName(string name);
        DeviceOffline GetByIp(string ip);
        List<DeviceOffline> GetAll();
        List<DeviceOffline> GetAllByCategory(string category);
        List<DeviceOffline> GetAllByPriority(string prioridad);

    }
}
