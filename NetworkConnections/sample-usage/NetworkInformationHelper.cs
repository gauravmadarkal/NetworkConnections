
using NetworkConnections.Interfaces;
using NetworkConnections.Models;
using NetworkConnections.Models.Wlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConnections.sample_usage
{
    public class NetworkInformationHelper
    {
        public void FetchNetworkInfo()
        {
            INetworkClient networkInformation = new NetworkInformation();
            ConnectionInfo connectionInfo = networkInformation.ConnectionInfo;
            string wifiSSID = connectionInfo.WlanInfo.SSID;
            bool isPasswordProtected = connectionInfo.WlanInfo.IsSecured;
            NetworkCategory category = connectionInfo.WlanInfo.NetworkCategory;
            bool isPublicNetwork = category == NetworkCategory.Public;
            string lanName = connectionInfo.LanInfo.Name;
        }
    }
}
