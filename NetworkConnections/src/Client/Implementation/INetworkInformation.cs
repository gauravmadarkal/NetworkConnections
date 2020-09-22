using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConnections.Client.Implementation
{
    interface INetworkInformation
    {
        ConnectionInfo ConnectionInfo { get; }
    }
}
