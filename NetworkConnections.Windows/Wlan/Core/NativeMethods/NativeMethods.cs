﻿using NetworkConnections.Windows.Wlan.Core.Enums;
using System;
using System.Runtime.InteropServices;

namespace NetworkConnections.Windows.Wlan.Core.NativeMethods
{
    internal static class NativeMethods
    {
        [DllImport("Wlanapi", EntryPoint = "WlanQueryInterface")]
        internal static extern uint WlanQueryInterface([In] IntPtr hClientHandle,
       [In] ref Guid pInterfaceGuid,
       WlanIntfOpcode OpCode,
       IntPtr pReserved,
       [Out] out uint pdwDataSize,
       ref IntPtr ppData,
       IntPtr pWlanOpcodeValueType);


        [DllImport("Wlanapi.dll")]
        internal static extern int WlanOpenHandle(
        uint dwClientVersion,
        IntPtr pReserved, //not in MSDN but required
        [Out] out uint pdwNegotiatedVersion,
        out IntPtr ClientHandle);

        [DllImport("Wlanapi", EntryPoint = "WlanEnumInterfaces")]
        internal static extern uint WlanEnumInterfaces([In] IntPtr hClientHandle, IntPtr pReserved, ref IntPtr ppInterfaceList);

        [DllImport("Wlanapi", EntryPoint = "WlanFreeMemory")]
        internal static extern void WlanFreeMemory([In] IntPtr pMemory);

        [DllImport("Wlanapi", EntryPoint = "WlanCloseHandle")]
        internal static extern uint WlanCloseHandle([In] IntPtr hClientHandle,
        IntPtr pReserved);
    }
}
