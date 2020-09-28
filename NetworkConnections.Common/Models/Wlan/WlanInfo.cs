using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConnections.Common.Models.Wlan
{
    /// <summary>
    /// Model class which represents the wireless 802.11 connection details
    /// </summary>
    public class WlanInfo
    {
        public string SSID { 
            get;
            set;
        }
        public bool IsSecured
        {
            get;
            set;
        }
        public NetworkCategory NetworkCategory
        {
            get;
             set;
        }
    }
}
