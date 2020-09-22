# NetworkConnections
[nuget-package -> Network Connections](https://www.nuget.org/packages/NetworkConnections/)
This package is used to simplify the process of fetching the network connection details of a system.
#### Current Support: Windows 7 or greater, DotNet framework 4.5 or greater
If you are trying to build a desktop application and you want fetch the network connection details, there isn't any easy way to fetch that.
Windows provides netsh commands which are not language locale neutral, 
NetworkConnections provides easy way to fetch WIFI and LAN connection details.
## Usage

> **Note:** Add NetworkConnections nuget package reference to your DotNet framework project

    INetworkInformation networkInformation = new NetworkInformation();
    ConnectionInfo connectionInfo = networkInformation.ConnectionInfo;
    string wifiSSID = connectionInfo.WlanInfo.SSID;
    bool isPasswordProtected = connectionInfo.WlanInfo.IsSecured;
    NetworkCategory category = connectionInfo.WlanInfo.NetworkCategory;
    bool isPublicNetwork = category == NetworkCategory.Public;
    string lanName = connectionInfo.LanInfo.Name;

It's pretty simple and straight forward

## Contribution
Feel free to contribute or suggest any enhancements

