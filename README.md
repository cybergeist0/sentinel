 # Sentinel - Server Audit & Monitoring Tool

 Sentinel is a Windows-based desktop app that scans your network for SQL Server machines or general Windows servers and retrieves:

 - Free disk space  
 - Memory usage  
 - OS version / Service Pack  
 - Uptime  
 - Maintenance status (custom)  
 - Host/IP details  

 ---

 ## Features

 - ✅ Network scan (manual & timer-based)
 - ✅ GUI list of active machines
 - ✅ CSV export of audit logs
 - ✅ Maintenance window support
 - ✅ WPF Desktop UI
 - ✅ `appsettings.json` for configuration

 ---

 ## Requirements

 - Windows 10+  
 - .NET Framework (WPF apps)  
 - Visual Studio 2022 (Community)  
 - PowerShell 5.1+  

 ---

 ## 🔧 Setup Instructions

 1. Clone the repo:

 ```bash
 git clone https://github.com/cybergeist0/sentinel.git
 ```

 2. Open `Sentinel.sln` in **Visual Studio 2022**

 3. Make sure `appsettings.json` is set to:
    - **Build Action**: `Content`
    - **Copy to Output Directory**: `Copy if newer`

 4. Right-click `GUI` → Set as Startup Project → Build & Run ✅

 5. On each target server, run `agent.ps1` (e.g. via service or scheduled task)

 ---

 ## Export Logs

 Click **“Export CSV”** to download the current audit log. Default export path is defined in `appsettings.json`.

 ---

 ## Security

 - PowerShell agents should only be deployed in trusted environments  
 - Credentials are not stored — agents are passive  
 - Encrypted config/comm can be added later  

