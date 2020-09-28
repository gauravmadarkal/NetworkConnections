using NetworkConnections.Common.Models.Lan.Core;
using NetworkConnections.Common.Models.Wlan;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConnections.Common.Models
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
