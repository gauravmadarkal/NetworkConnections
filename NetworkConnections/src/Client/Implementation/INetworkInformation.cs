using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConnections.Client.Implementation
{
    /// <summary>
    /// fetches the connected network details
    /// </summary>
    interface INetworkInformation
    {
        /// <summary>
        /// this property has details about the lan and wifi connection
        /// </summary>
        ConnectionInfo ConnectionInfo { get; }
    }
}
