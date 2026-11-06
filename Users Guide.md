# NetScan: User's Guide



## Downloads & Runtimes

Download the latest release from the [Releases page](https://github.com/RemusRigo/NetScan-VB.NET/releases).

Each release contains one folder per framework and architecture:

|Folder|Target Framework|Architecture|Required Runtime|
|-|-|-|-|
|`net8.0 x64`|.NET 8.0|64-bit|[.NET 8.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net8.0 x86`|.NET 8.0|32-bit|[.NET 8.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net10.0 x64`|.NET 10.0|64-bit|[.NET 10.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/10.0)|
|`net10.0 x86`|.NET 10.0|32-bit|[.NET 10.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/10.0)|

> **Which one do I need?** On 64-bit Windows 10/11, use **x64**. Pick the .NET version whose
> **Desktop Runtime** you already have installed, or install one from the links above.
> The plain ".NET Runtime" is not enough; NetScan needs the **Desktop** Runtime.

To check which runtimes are installed, open a command prompt and run:

```
dotnet --list-runtimes
```

Look for a `Microsoft.WindowsDesktop.App 8.x` or `10.x` line.



## Install

NetScan is portable: there is no installer.

1. Download and extract the folder that matches your system (see the table above), e.g. `net8.0 x64`.
2. Copy the **whole folder** to a location where you have write access, for example:
   `D:\My Apps\NetScan\`
   The folder must contain all 4 files; NetScan won't start if any of them is missing:
   * `NetScan.exe`
   * `NetScan.dll`
   * `NetScan.deps.json`
   * `NetScan.runtimeconfig.json`
3. Optional: copy `oui.csv` into the same folder to see device vendors (see *Vendor lookup* below).
4. Double-click `NetScan.exe`.

> NetScan saves its settings and presets next to `NetScan.exe`, so don't put it in a read-only
> folder such as `C:\Program Files\`.

> If Windows blocks the file ("Windows protected your PC"), right-click `NetScan.exe` →
> **Properties** → check **Unblock** → **OK**, then try again.



## Scan your network

1. On start, the **IP Range** box is filled with the local network of every active network interface,
   e.g. `192.168.1.1-255`.
2. Edit the ranges if needed (see *IP ranges* below).
3. Click **Scan**.

Every IP in the ranges is listed as *Pending*, then updated to *Online* or *Offline* as the scan runs.
The progress bar at the bottom and the title bar show how far the scan is.

|Column|Description|
|-|-|
|**Status**|*Pending*, *Online*, *Offline*, or *Skipped* (scan was stopped before this IP)|
|**IP Address**|The scanned IP|
|**Hostname**|Name returned by DNS, or *Unknown*|
|**MAC Address**|Found with ARP; only works for devices on your local subnet|
|**Vendor**|Network card manufacturer, from the MAC address (see *Vendor lookup*)|
|**TTL**|Time To Live of the ping reply|
|**Type**|Probable operating system, guessed from the TTL (e.g. 128 = Windows, 64 = Linux/Android/macOS)|

> Devices that block ping (for example, by a firewall) are shown as *Offline*.



## IP ranges

Enter one range per line, in the form `first-three-octets.from-to`:

```
192.168.1.1-255
10.0.0.100-150
```

Only the last octet can be a range. If any line is not valid, the scan doesn't start and *Invalid range* is shown.

Right-click the **IP Range** box for more options:

|Menu|Description|
|-|-|
|**Rescan**|Fill the box again with the local networks of all active network interfaces|
|**Save as preset**|Save the current ranges to a `.preset` file|
|**Load preset**|Replace the current ranges with the ones from a `.preset` file|

Presets are saved by default in the `Presets` folder next to `NetScan.exe`.



## Toolbar

|Button|Description|
|-|-|
|**Scan**|Start scanning the IP ranges|
|**Pause**|Pause / resume the scan; pings already sent finish, no new ones start. The progress bar turns orange while paused|
|**Stop**|Stop the scan; pings already sent finish, the remaining IPs are marked *Skipped*|
|**Hide Offline**|Remove offline IPs from the list during the scan|
|**Hide Hostname**|Hide the Hostname column and skip the DNS lookup (faster)|
|**Hide MAC**|Hide the MAC Address column and skip the ARP lookup (faster)|
|**Hide Vendor**|Hide the Vendor column and skip the vendor lookup|

> Set **Hide Offline**, **Hide Hostname**, **Hide MAC** and **Hide Vendor** before you click **Scan**:
> they apply to IPs scanned after you click them.

Right-click the title bar → **About...** for version information.



## Vendor lookup

The vendor is looked up offline, from the first 6 hex digits of the MAC address (the OUI),
in a file named `oui.csv` next to `NetScan.exe`. The file has 2 columns:

```
Assignment,Organization Name
286FB9,"Nokia Shanghai Bell Co., Ltd."
```

When the vendor is not found, *Unknown* is shown. The title bar shows how many OUI entries were loaded.
The OUI list is published by the IEEE: [oui.csv](https://standards-oui.ieee.org/oui/oui.csv).
The IEEE file has more columns (it starts with *Registry*); keep only *Assignment* and *Organization Name*.



## Uninstall

1. Close NetScan.
2. Delete the folder you copied in step 2 of *Install* (this also deletes your settings and presets).
