using System.Collections.Generic;
using System.IO;
using GUI.Models;

namespace GUI.Services
{
    public class ExportService
    {
        public void ExportCsv(string filePath, IEnumerable<ServerInfo> servers)
        {
            var config = new ConfigurationService();
            if (string.IsNullOrWhiteSpace(filePath))
                filePath = config.DefaultExportPath;

            using var writer = new StreamWriter(filePath);
            writer.WriteLine("IP Address,Hostname,Status,OS Version,Uptime (hrs),Free Disk (GB)");

            foreach (var server in servers)
            {
                string status = server.IsInMaintenance ? "In Maintenance" : "Active";
                writer.WriteLine($"{server.Ip},{server.Hostname},{status},{server.OSVersion},{server.UptimeHours},{server.FreeDiskGB}");
            }
        }

    }
}
