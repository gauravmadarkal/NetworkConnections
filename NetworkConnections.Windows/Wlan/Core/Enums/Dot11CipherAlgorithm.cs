﻿namespace NetworkConnections.Windows.Wlan.Core.Enums
{
    internal enum Dot11CipherAlgorithm
    {

        /// DOT11_CIPHER_ALGO_NONE -> 0x00
        DOT11_CIPHER_ALGO_NONE = 0,

        /// DOT11_CIPHER_ALGO_WEP40 -> 0x01
        DOT11_CIPHER_ALGO_WEP40 = 1,

        /// DOT11_CIPHER_ALGO_TKIP -> 0x02
        DOT11_CIPHER_ALGO_TKIP = 2,

        /// DOT11_CIPHER_ALGO_CCMP -> 0x04
        DOT11_CIPHER_ALGO_CCMP = 4,

        /// DOT11_CIPHER_ALGO_WEP104 -> 0x05
        DOT11_CIPHER_ALGO_WEP104 = 5,

        /// DOT11_CIPHER_ALGO_WPA_USE_GROUP -> 0x100
        DOT11_CIPHER_ALGO_WPA_USE_GROUP = 256,

        /// DOT11_CIPHER_ALGO_RSN_USE_GROUP -> 0x100
        DOT11_CIPHER_ALGO_RSN_USE_GROUP = 256,

        /// DOT11_CIPHER_ALGO_WEP -> 0x101
        DOT11_CIPHER_ALGO_WEP = 257,

        /// DOT11_CIPHER_ALGO_IHV_START -> 0x80000000
        DOT11_CIPHER_ALGO_IHV_START = -2147483648,

        /// DOT11_CIPHER_ALGO_IHV_END -> 0xffffffff
        DOT11_CIPHER_ALGO_IHV_END = -1,
    }
}