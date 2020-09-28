using NetworkConnections.Common.Interfaces;
using NetworkConnections.Common.Models;
using NetworkConnections.Windows.Implementation;
using System;
using System.Runtime.InteropServices;

namespace NetworkConnections
{
    public class NetworkInformation : INetworkClient
    {
        public ConnectionInfo ConnectionInfo
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    INetworkClient networkClient = new WindowsNetworkClient();
                    return networkClient.ConnectionInfo;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    //INetworkClient networkClient = new LinuxNetworkClient();
                    //return networkClient.ConnectionInfo;
                    return new ConnectionInfo();
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    //INetworkClient networkClient = new OSXNetworkClient();
                    //return networkClient.ConnectionInfo;
                    return new ConnectionInfo();
                }
                //unknown platform
                return new ConnectionInfo();
            }
        }
    }
}
