using Application.Interfaces;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogs
{
    public class Program
    {
        static void Main(string[] args)
        {
           MonitoringServices logsReg = new MonitoringServices();
           Console.ReadKey();
        }
    }
}
