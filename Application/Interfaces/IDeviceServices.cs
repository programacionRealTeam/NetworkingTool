﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDeviceServices
    {
        DeviceResponse ping(Device device);

    }
}
