# NetScan
NetScan: a network scanner utility



## Author

* [@remusrigo](https://github.com/RemusRigo)
* [@remusrigo](https://sourceforge.net/u/remusrigo/profile)



## Features

* Detects the local IPv4 networks of all active network interfaces (ignores loopback, inactive interfaces, IPv6 and Link-local/APIPA 169.254.x.x)
* Scan multiple IP ranges at once (one range per line)
* Save/Load IP ranges as presets
* Display online/offline status of every IP
* Display/hide Hostname
* Display/hide MAC Address
* Display/hide Vendor (offline lookup, IEEE OUI table)
* Display TTL and the probable device OS
* Hide offline IP's
* Pause/Stop scanning (on pause current threads continue and no new ones will start)



## User's Guide

See the [User's Guide](UsersGuide.md) for downloads, runtimes, installation and usage.



## Statistics

![VB.NET](https://img.shields.io/badge/Language-VB.NET-512BD4?labelColor=555555)
![VS](https://img.shields.io/badge/IDE-Visual%20Studio-5C2D91?labelColor=555555)
![CSV](https://img.shields.io/badge/File-CSV-5C2D91?labelColor=555555)
<br>
![status](https://img.shields.io/badge/status-completed-brightgreen) 
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/RemusRigo/NetScan-VB.NET/total)



## Roadmap

* 2026-10-07: Upd: ignore Link-local/APIPA (169.254.x.x)
* 2026-10-03: Add: Implement Pause/Stop
* 2026-10-02: Add: Save/Load IP Ranges as presets
* 2026-10-02: Upd: Change button size and icons
* 2026-10-01: Upd: TTL values
* 2026-10-01: Upd: Scan all Network Interfaces and extract local IP (ignore loopbacks, inactive network and IPv6)
* 2026-08-20: Add: Device OS
* 2026-08-20: Add: TTL
* 2026-08-04: Add: About form
* 2026-08-04: Add: [Radar Icon](https://www.flaticon.com/free-icon/radar_6989456) by [bzzrincantation](https://www.flaticon.com/authors/bzzrincantation)
* 2026-08-04: Add: Button icons
* 2026-08-03: Add: Vendor (offline)
* 2026-08-03: Add: Vendor (online version fails, Too Many Requests error)
* 2026-08-03: Add: Hostname
* 2026-08-03: Add: MAC Address
* 2026-08-02: Add: hide Offline IP's
* 2026-08-02: Add: show IP status
* 2026-08-02: Add: scan IP range
* 2026-08-02: Project started



## Resources

* [OUI Standards](https://standards-oui.ieee.org/oui/oui.csv)
* [MAC Address Vendors](https://macvendors.com)
