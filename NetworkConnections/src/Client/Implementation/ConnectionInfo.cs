using NetworkConnections.Lan.Core;
using NetworkConnections.Wlan.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConnections.Client.Implementation
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
