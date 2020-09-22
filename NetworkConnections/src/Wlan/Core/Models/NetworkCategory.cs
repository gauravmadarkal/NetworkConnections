using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConnections.src.Wlan.Core.Models
{
    /// <summary>
    /// Specifies the trust level for a network.
    /// </summary>
    public enum NetworkCategory
    {
        /// <summary>
        /// The network is a public (untrusted) network.
        /// </summary>        
        Public = 0,
        /// <summary>
        /// The network is a private (trusted) network.
        /// </summary>        
        Private = 1,
        /// <summary>
        /// The network is authenticated against an Active Directory domain.
        /// </summary>        
        Authenticated = 2
    }
}
