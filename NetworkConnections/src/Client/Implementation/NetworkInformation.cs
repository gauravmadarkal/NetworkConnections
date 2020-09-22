﻿using Microsoft.WindowsAPICodePack.Net;
using NetworkConnections.src.Lan.Core;
using NetworkConnections.src.Wlan.Core.Models;
using NetworkConnections.src.Wlan.Core.NativeMethods;
using NetworkConnections.src.Wlan.Core.Structs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace NetworkConnections.src.Client.Implementation
{
    public class NetworkInformation
    {
        public ConnectionInfo ConnectionInfo
        {
            get
            {
                return GetConnectionDetails();
            }
        }
        
        internal ConnectionInfo GetConnectionDetails()
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
                NetworkInterface[] networkInterfaces = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(ninterface => ninterface.OperationalStatus == OperationalStatus.Up
                            && ninterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel 
                            && ninterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .ToArray();
                foreach (NetworkInterface netInterface in networkInterfaces)
                {
                    foreach (Microsoft.WindowsAPICodePack.Net.Network network in networks)
                    {
                        foreach (NetworkConnection conn in network.Connections)
                        {
                            string id = netInterface.Id;
                            id = id.Substring(1, id.Length - 2);
                            id = id.ToUpper(CultureInfo.InvariantCulture);

                            if (id.Equals(conn.AdapterId.ToString().ToUpper(CultureInfo.InvariantCulture), StringComparison.InvariantCultureIgnoreCase)
                                && adapters.Contains(id))
                            {
                                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                                {
                                    networkInfo.WlanInfo = GetWirelessConnection();
                                    networkInfo.WlanInfo.NetworkCategory = conn.Network.Category;
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

        internal List<string> GetActivePhysicalAdapters()
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
                        //Console.WriteLine(pd.Name + ": " + (pd.Value ?? "N/A"));
                        string id = pd.Value.ToString();
                        id = id.Substring(1, id.Length - 2);
                        id = id.ToUpper(CultureInfo.InvariantCulture);
                        adapterIds.Add(id);
                    }
                }
                //Console.WriteLine("\n\n");
            }
            return adapterIds;
        }


        /// <summary>
        /// This function fetches the ssid of wlan connection for the system
        /// </summary>
        /// <returns>A wifiInfo object which contains SSID and authenticated flag</returns>
        public WlanInfo GetWirelessConnection()
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