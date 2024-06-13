using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDeviceQuery
    {
        Device GetByName(string name);
        Device GetByIp(string ip);
        List<Device> GetAll();
        List<Device> GetAllByCategory(string category);
        List<Device> GetAllByPriority(string prioridad);
    }
}

