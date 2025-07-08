$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$agentScript = Join-Path $scriptDir 'AgentService.ps1'

New-Service `
  -Name "DBACS-Agent" `
  -BinaryPathName "powershell -File `"$agentScript`"" `
  -Description "DBACS Sentinel PowerShell Agent" `
  -StartupType Automatic

Start-Service "DBACS-Agent"
Write-Host "Service 'DBACS-Agent' installed and started."
