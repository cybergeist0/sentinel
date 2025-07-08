$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

$configPath = Join-Path $scriptDir 'config.json'
if (-Not (Test-Path $configPath)) {
    Write-Error "Missing config.json in $scriptDir"
    exit 1
}
$config = Get-Content $configPath -Raw | ConvertFrom-Json
$Port  = $config.port

$maintenanceFile = Join-Path $scriptDir 'maintenance.json'
$maintenance = @{ start_time = $null; end_time = $null }
if (Test-Path $maintenanceFile) {
    $json = Get-Content $maintenanceFile -Raw | ConvertFrom-Json
    $maintenance.start_time = $json.start_time
    $maintenance.end_time   = $json.end_time
}

$listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Any, $Port)
$listener.Start()
Write-Host "Agent listening on port $Port..."

while ($true) {
    try {
        $client = $listener.AcceptTcpClient()
        $stream = $client.GetStream()

        $hostname    = $env:COMPUTERNAME
        $ipObj       = Get-NetIPAddress -AddressFamily IPv4 |
                       Where-Object { $_.IPAddress -ne '127.0.0.1' } |
                       Select-Object -First 1
        $ip          = $ipObj.IPAddress
        $os          = (Get-CimInstance Win32_OperatingSystem).Caption
        $uptimeHours = [math]::Round(((Get-Date) - (Get-CimInstance Win32_OperatingSystem).LastBootUpTime).TotalHours)
        $memGB       = [math]::Round((Get-CimInstance Win32_ComputerSystem).TotalPhysicalMemory / 1GB, 2)
        $diskGB      = [math]::Round((Get-CimInstance Win32_LogicalDisk -Filter "DriveType=3").FreeSpace / 1GB, 2)

        $payload = [PSCustomObject]@{
            id            = $hostname
            hostname      = $hostname
            ip            = $ip
            os_version    = $os
            sql_version   = $null
            sql_memory_mb = $null
            free_space_gb = $diskGB
            memory_gb     = $memGB
            uptime_hours  = $uptimeHours
            maintenance   = $maintenance
        } | ConvertTo-Json -Depth 4

        $bytes = [System.Text.Encoding]::UTF8.GetBytes($payload)
        $stream.Write($bytes, 0, $bytes.Length)
        $client.Close()
    }
    catch {
        Write-Warning "Error handling client: $_"
    }
}
