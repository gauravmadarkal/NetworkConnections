using NetworkConnections.Wlan.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConnections.Wlan.Core.Structs
{
    internal class Structs
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WlanInterfaceInfo
        {
            /// GUID->_GUID
            internal Guid InterfaceGuid;

            /// WCHAR[256]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string strInterfaceDescription;

            /// WLAN_INTERFACE_STATE->_WLAN_INTERFACE_STATE
            internal WlanInterfaceState isState;
        }
        /// <summary>
        /// Contains an array of NIC information
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct WlanInterfaceInfoList
        {
            /// <summary>
            /// Length of <see cref="InterfaceInfo"/> array
            /// </summary>
            internal Int32 dwNumberOfItems;
            /// <summary>
            /// This member is not used by the wireless service. Applications can use this member when processing individual interfaces.
            /// </summary>
            internal Int32 dwIndex;
            /// <summary>
            /// Array of WLAN interfaces.
            /// </summary>
            internal WlanInterfaceInfo[] InterfaceInfo;

            /// <summary>
            /// Constructor for WLAN_INTERFACE_INFO_LIST.
            /// Constructor is needed because the InterfaceInfo member varies based on how many adapters are in the system.
            /// </summary>
            /// <param name="pList">the unmanaged pointer containing the list.</param>
            internal WlanInterfaceInfoList(IntPtr pList)
            {
                // The first 4 bytes are the number of WLAN_INTERFACE_INFO structures.
                dwNumberOfItems = Marshal.ReadInt32(pList, 0);

                // The next 4 bytes are the index of the current item in the unmanaged API.
                dwIndex = Marshal.ReadInt32(pList, 4);

                // Construct the array of WLAN_INTERFACE_INFO structures.
                InterfaceInfo = new WlanInterfaceInfo[dwNumberOfItems];

                for (int i = 0; i <= dwNumberOfItems - 1; i++)
                {
                    // The offset of the array of structures is 8 bytes past the beginning.
                    // Then, take the index and multiply it by the number of bytes in the
                    // structure.
                    // The length of the WLAN_INTERFACE_INFO structure is 532 bytes - this
                    // was determined by doing a Marshall.SizeOf(WLAN_INTERFACE_INFO)
                    IntPtr pItemList = new IntPtr(pList.ToInt64() + (i * 532) + 8);

                    // Construct the WLAN_INTERFACE_INFO structure, marshal the unmanaged
                    // structure into it, then copy it to the array of structures.
                    InterfaceInfo[i] = (WlanInterfaceInfo)Marshal.PtrToStructure(pItemList, typeof(WlanInterfaceInfo));
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct Dot11Ssid
        {

            /// ULONG->unsigned int
            internal uint uSSIDLength;

            /// UCHAR[]
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //internal string ucSSID; 

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.AnsiBStr, SizeConst = 32)]
            internal byte[] ucSSID;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct WlanAssociationAttributes
        {

            /// DOT11_SSID->_DOT11_SSID
            internal Dot11Ssid dot11Ssid;

            /// DOT11_BSS_TYPE->_DOT11_BSS_TYPE
            internal Dot11BssType dot11BssType;

            /// DOT11_MAC_ADDRESS->UCHAR[6]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            internal string dot11Bssid;

            /// DOT11_PHY_TYPE->_DOT11_PHY_TYPE
            internal Dot11PhyType dot11PhyType;

            /// ULONG->unsigned int
            internal uint uDot11PhyIndex;

            /// WLAN_SIGNAL_QUALITY->ULONG->unsigned int
            internal uint wlanSignalQuality;

            /// ULONG->unsigned int
            internal uint ulRxRate;

            /// ULONG->unsigned int
            internal uint ulTxRate;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct WlanSecurityAttributes
        {
            /// <summary>
            /// BOOL->int
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            internal bool bSecurityEnabled;

            /// <summary>
            /// BOOL->int
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            internal bool bOneXEnabled;

            /// <summary>
            /// DOT11_AUTH_ALGORITHM->_DOT11_AUTH_ALGORITHM
            /// </summary>
            internal Dot11AuthAlgorithm dot11AuthAlgorithm;

            /// <summary>
            /// DOT11_CIPHER_ALGORITHM->_DOT11_CIPHER_ALGORITHM
            /// </summary>
            internal Dot11CipherAlgorithm dot11CipherAlgorithm;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WlanConnectionAttributes
        {

            /// WLAN_INTERFACE_STATE->_WLAN_INTERFACE_STATE
            internal WlanInterfaceState isState;

            /// WLAN_CONNECTION_MODE->_WLAN_CONNECTION_MODE
            internal WlanConnectionMode wlanConnectionMode;

            /// WCHAR[256]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string strProfileName;

            /// WLAN_ASSOCIATION_ATTRIBUTES->_WLAN_ASSOCIATION_ATTRIBUTES
            internal WlanAssociationAttributes wlanAssociationAttributes;

            /// WLAN_SECURITY_ATTRIBUTES->_WLAN_SECURITY_ATTRIBUTES
            internal WlanSecurityAttributes wlanSecurityAttributes;
        }

    }
}
