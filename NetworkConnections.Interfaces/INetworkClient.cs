using NetworkConnections.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConnections.Interfaces
{
    /// <summary>
    /// fetches the connected network details
    /// </summary>
    public interface INetworkClient
    {
        /// <summary>
        /// this property has details about the lan and wifi connection
        /// </summary>
        ConnectionInfo ConnectionInfo { get; }
    }
}
