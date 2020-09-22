using NetworkConnections.src.Lan.Core;
using NetworkConnections.src.Wlan.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConnections.src.Client.Implementation
{
    public class ConnectionInfo
    {
        public LanInfo LanInfo
        {
            get;
            internal set;
        }
        public WlanInfo WlanInfo
        {
            get;
            internal set;
        }
    }
}
