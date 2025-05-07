# 🔍 Multi-Threaded Port Scanner

![.NET](https://img.shields.io/badge/.NET-6.0+-purple?logo=dotnet)
![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Linux%20%7C%20MacOS-blue)
![License](https://img.shields.io/badge/License-MIT-green)

> A fast and colorful multi-threaded port scanner written in **C#**, built to quickly identify open TCP ports on any IPv4 or IPv6 address.

---

## ✨ Features

- ✅ Multi-threaded scanning (async + `SemaphoreSlim`)
- ✅ Clean and colorful console interface
- ✅ Customizable port range input
- ✅ Detection of:
  - Open ports
  - Filtered (timed out) ports
  - Closed ports
- ✅ Console title, custom ASCII banner, and status logs
- ✅ Works on both IPv4 and IPv6 targets

---

## 🖥️ Example Output

Enter target IP: 192.168.1.1
Enter start port (default 1): 20
Enter end port (default 80):

Scanning 192.168.1.1 from port 20 to 80...

[✔] Port 22 is OPEN
[✔] Port 80 is OPEN
[✘] Port 23 is CLOSED
[⚠] Port 25 is FILTERED (Connection Timeout)

Scan Complete!

Open Ports:
Port 22 is OPEN
Port 80 is OPEN
-----


---

## ⚙️ How to Use

### 🧾 Prerequisites

- [.NET SDK 6.0 or later](https://dotnet.microsoft.com/en-us/download)

### 🛠️ Build & Run

```bash
git clone https://github.com/sn1wfy/PortScanner.git
cd PortScanner
dotnet run
```
#This tool is intended for educational and authorized penetration testing purposes only. Scanning systems without explicit permission is illegal and unethical.

