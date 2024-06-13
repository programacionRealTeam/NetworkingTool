using Application.Response;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDeviceCommand
    {
        Task Insert(Device device);
        Task Update(Device device);
        Task Delete(string name);
    }
}
