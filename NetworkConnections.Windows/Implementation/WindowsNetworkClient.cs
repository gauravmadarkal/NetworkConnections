using Microsoft.WindowsAPICodePack.Net;
using NetworkConnections.Interfaces;
using NetworkConnections.Lan.Core;
using NetworkConnections.Models;
using NetworkConnections.Models.Wlan;
using NetworkConnections.Windows.Wlan.Core.NativeMethods;
using NetworkConnections.Windows.Wlan.Core.Structs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace NetworkConnections.Windows.Implementation
{
    /// <summary>
    /// this class fetches the connected network details
    /// </summary>
    public class WindowsNetworkClient : INetworkClient
    {
        
        /// <summary>
        /// constructor
        /// </summary>
        public WindowsNetworkClient() { }

        /// <summary>
        /// this property has details about the lan and wifi connection
        /// </summary>
        public ConnectionInfo ConnectionInfo
        {
            get
            {
                return GetConnectionDetails();
            }
        }
        
        /// <summary>
        /// this method fetches the active connection details
        /// </summary>
        /// <returns>object containing wlaninfo and laninfo</returns>
        private ConnectionInfo GetConnectionDetails()
        {
            ConnectionInfo networkInfo = new ConnectionInfo
            {
                LanInfo = new LanInfo(),
                WlanInfo = new WlanInfo()
            };
            try
            {
                List<string> adapters = GetActivePhysicalAdapters();
                var networks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);
                // fetch all networks adapters that are active and running, 
                // adapters which are not loopback or tunnel
                NetworkInterface[] networkInterfaces = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(ninterface => ninterface.OperationalStatus == OperationalStatus.Up
                            && ninterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel 
                            && ninterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .ToArray();
                foreach (NetworkInterface netInterface in networkInterfaces)
                {
                    foreach (Network network in networks)
                    {
                        foreach (NetworkConnection conn in network.Connections)
                        {
                            string id = netInterface.Id;
                            id = id.Substring(1, id.Length - 2);
                            id = id.ToUpper(CultureInfo.InvariantCulture);

                            // compare the adapterid to fetch the right connection name
                            if (id.Equals(conn.AdapterId.ToString().ToUpper(CultureInfo.InvariantCulture), StringComparison.InvariantCultureIgnoreCase)
                                && adapters.Contains(id))
                            {
                                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                                {
                                    networkInfo.WlanInfo = GetWirelessConnection();
                                    networkInfo.WlanInfo.NetworkCategory = GetNetworkCategory(conn.Network.Category);
                                    if (networkInfo.WlanInfo == null)
                                    {
                                        networkInfo.WlanInfo.SSID = conn.Network.Name;
                                        networkInfo.WlanInfo.IsSecured = false;
                                    }
                                }
                                else
                                {
                                    networkInfo.LanInfo.Name = conn.Network.Name;
                                }
                            }
                        }
                    }                    
                }
                return networkInfo;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return networkInfo;
            }
        }

        /// <summary>
        /// mapper method
        /// it takes the api code pack enum and returns a custom enum
        /// </summary>
        /// <param name="category">API code pack enum</param>
        /// <returns>Network Cateory enum</returns>
        private Models.Wlan.NetworkCategory GetNetworkCategory(Microsoft.WindowsAPICodePack.Net.NetworkCategory category)
        {
            switch (category)
            {
                case Microsoft.WindowsAPICodePack.Net.NetworkCategory.Public:
                    return Models.Wlan.NetworkCategory.Public;
                case Microsoft.WindowsAPICodePack.Net.NetworkCategory.Private:
                    return Models.Wlan.NetworkCategory.Private;
                case Microsoft.WindowsAPICodePack.Net.NetworkCategory.Authenticated:
                    return Models.Wlan.NetworkCategory.Authenticated;
                default:
                    return Models.Wlan.NetworkCategory.Private;
            }
        }

        /// <summary>
        /// This function gets all the adapters which are not virtual, loopback, tunnel or default
        /// </summary>
        /// <returns>a list of actively enabled adapters</returns>
        private List<string> GetActivePhysicalAdapters()
        {
            List<string> adapterIds = new List<string>();
            ManagementObjectSearcher mos = null;
            // Final solution with filtering on the Manufacturer and PNPDeviceID not starting with "ROOT\"
            // Physical devices have PNPDeviceID starting with "PCI\" or something else besides "ROOT\"
            mos = new ManagementObjectSearcher(@"SELECT * 
                                     FROM   Win32_NetworkAdapter 
                                     WHERE  Manufacturer != 'Microsoft' 
                                            AND NOT PNPDeviceID LIKE 'ROOT\\%'");
            // Get the physical adapters and sort them by their index. 
            // This is needed because they're not sorted by default
            IList<ManagementObject> managementObjectList = mos.Get()
                                                              .Cast<ManagementObject>()
                                                              .OrderBy(p => Convert.ToUInt32(p.Properties["Index"].Value))
                                                              .ToList();

            // Let's just show all the properties for all physical adapters.
            foreach (ManagementObject mo in managementObjectList)
            {
                foreach (PropertyData pd in mo.Properties)
                {
                    if (pd.Name.Equals("GUID"))
                    {
                        string id = pd.Value.ToString();
                        id = id.Substring(1, id.Length - 2);
                        id = id.ToUpper(CultureInfo.InvariantCulture);
                        adapterIds.Add(id);
                    }
                }
            }
            return adapterIds;
        }


        /// <summary>
        /// This function fetches the ssid of wlan connection for the system
        /// </summary>
        /// <returns>A wlaninfo object which contains SSID and security info</returns>
        private WlanInfo GetWirelessConnection()
        {
            WlanInfo wifiInfo = new WlanInfo();
            IntPtr handle = IntPtr.Zero;
            uint negotiatedVersion;
            try
            {
                if (NativeMethods.WlanOpenHandle(1, IntPtr.Zero, out negotiatedVersion, out handle) != 0)
                    return null;

                IntPtr ptr = new IntPtr();
                if (NativeMethods.WlanEnumInterfaces(handle, IntPtr.Zero, ref ptr) != 0)
                    return null;

                Structs.WlanInterfaceInfoList infoList = new Structs.WlanInterfaceInfoList(ptr);
                NativeMethods.WlanFreeMemory(ptr);

                Guid guid;
                uint dataSize;
                Structs.WlanConnectionAttributes connection;
                // Call wlanqueryinterface for all the interfaces in the list
                for (int i = 0; i < infoList.dwNumberOfItems; i++)
                {
                    guid = infoList.InterfaceInfo[i].InterfaceGuid;
                    if (NativeMethods.WlanQueryInterface(handle, ref guid, Wlan.Core.Enums.WlanIntfOpcode.wlan_intf_opcode_current_connection, IntPtr.Zero, out dataSize, ref ptr, IntPtr.Zero) != 0)
                        return null;

                    connection = (Structs.WlanConnectionAttributes)Marshal.PtrToStructure(ptr, typeof(Structs.WlanConnectionAttributes));
                    byte[] arr = connection.wlanAssociationAttributes.dot11Ssid.ucSSID;
                    string ssid = Encoding.UTF8.GetString(arr);
                    wifiInfo.SSID = ssid.Trim('\0');
                    wifiInfo.IsSecured = connection.wlanSecurityAttributes.bSecurityEnabled;
                    NativeMethods.WlanFreeMemory(ptr);
                }
                return wifiInfo;
            }
#pragma warning disable CA1031 // pinvokes might throw runtime exception
            catch (Exception)
#pragma warning restore CA1031 
            {
                return null;
            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    uint returncode = NativeMethods.WlanCloseHandle(handle, IntPtr.Zero);
                }
            }
        }
    }
}
