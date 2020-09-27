using NetworkConnections.Lan.Core;
using NetworkConnections.Models.Wlan;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConnections.Models
{
    public class ConnectionInfo
    {
        public LanInfo LanInfo
        {
            get;
            set;
        }
        public WlanInfo WlanInfo
        {
            get;
            set;
        }
    }
}
