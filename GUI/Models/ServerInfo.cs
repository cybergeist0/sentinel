using System;

namespace GUI.Models
{
    public class ServerInfo
    {
        public string Id { get; set; }
        public string Hostname { get; set; }
        public string Ip { get; set; }
        public string OSVersion { get; set; }
        public double FreeDiskGB { get; set; }
        public double MemoryGB { get; set; }
        public int UptimeHours { get; set; }
        public DateTime MaintStart { get; set; }
        public DateTime MaintEnd { get; set; }
        public bool IsInMaintenance =>
            DateTime.UtcNow >= MaintStart && DateTime.UtcNow <= MaintEnd;
    }
}
