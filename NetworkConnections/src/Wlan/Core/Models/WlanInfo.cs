using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConnections.Wlan.Core.Models
{
    /// <summary>
    /// Model class which represents the wireless 802.11 connection details
    /// </summary>
    public class WlanInfo
    {
        public string SSID { 
            get;
            internal set;
        }
        public bool IsSecured
        {
            get;
            internal set;
        }
        public NetworkCategory NetworkCategory
        {
            get;
            internal set;
        }
    }
}
