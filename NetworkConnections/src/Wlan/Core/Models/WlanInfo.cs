using Microsoft.WindowsAPICodePack.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConnections.src.Wlan.Core.Models
{
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
