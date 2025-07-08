using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GUI.Models;

namespace GUI.Services
{
    public static class NetworkScanner
    {
        private class AgentPayload
        {
            public string id;
            public string hostname;
            public string ip;
            public string os_version;
            public double free_space_gb;
            public double memory_gb;
            public int uptime_hours;
            public Maintenance maintenance;
        }
        private class Maintenance { public DateTime start_time; public DateTime end_time; }

        public static async Task<List<ServerInfo>> ScanAsync(string subnet, int port)
        {
            var result = new List<ServerInfo>();
            var hosts = await DiscoverAsync(subnet, port);

            foreach (var ip in hosts)
            {
                try
                {
                    using var tcp = new TcpClient();
                    await tcp.ConnectAsync(ip, port);
                    var buf = new byte[8192];
                    var len = await tcp.GetStream().ReadAsync(buf, 0, buf.Length);
                    var json = Encoding.UTF8.GetString(buf, 0, len);
                    var pl = JsonConvert.DeserializeObject<AgentPayload>(json);

                    result.Add(new ServerInfo
                    {
                        Id = pl.id,
                        Hostname = pl.hostname,
                        Ip = pl.ip,
                        OSVersion = pl.os_version,
                        FreeDiskGB = pl.free_space_gb,
                        MemoryGB = pl.memory_gb,
                        UptimeHours = pl.uptime_hours,
                        MaintStart = pl.maintenance.start_time,
                        MaintEnd = pl.maintenance.end_time
                    });
                }
                catch {}
            }

            return result;
        }

        private static async Task<List<string>> DiscoverAsync(string subnet, int port)
        {
            var active = new List<string>();
            var tasks = new List<Task>();
            for (int i = 1; i < 255; i++)
            {
                var ip = $"{subnet}.{i}";
                tasks.Add(CheckAsync(ip, port, active));
            }
            await Task.WhenAll(tasks);
            return active;
        }

        private static async Task CheckAsync(string ip, int port, List<string> active)
        {
            try
            {
                using var ping = new Ping();
                var rep = await ping.SendPingAsync(ip, 200);
                if (rep.Status == IPStatus.Success)
                {
                    using var client = new TcpClient();
                    var t = client.ConnectAsync(ip, port);
                    if (await Task.WhenAny(t, Task.Delay(200)) == t && client.Connected)
                        lock (active) { active.Add(ip); }
                }
            }
            catch { }
        }
    }
}
