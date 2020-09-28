using NetworkConnections.Common.Interfaces;
using NetworkConnections.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConnections.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("started: " + System.DateTime.Now.ToLongTimeString());
            INetworkClient networkClient = new NetworkInformation();
            ConnectionInfo connectionInfo = networkClient.ConnectionInfo;
            Console.WriteLine("Hello World!, " + connectionInfo.WlanInfo.SSID);
            Console.WriteLine("end: " + System.DateTime.Now.ToLongTimeString());
            Console.ReadLine();
        }
    }
}
