using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Scanner
{
    public class ScannerService
    {
        private readonly string _subnet;
        private readonly int _port;

        public ScannerService(string subnet, int port = 5005)
        {
            _subnet = subnet;
            _port = port;
        }

        public async Task<List<string>> DiscoverAgentsAsync()
        {
            var activeHosts = new List<string>();
            var tasks = new List<Task>();

            for (int i = 1; i < 255; i++)
            {
                string ip = $"{_subnet}.{i}";
                tasks.Add(CheckHostAsync(ip, activeHosts));
            }

            await Task.WhenAll(tasks);
            return activeHosts;
        }

        private async Task CheckHostAsync(string ip, List<string> active)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(ip, 200);
                if (reply.Status == IPStatus.Success)
                {
                    using var client = new TcpClient();
                    var connectTask = client.ConnectAsync(ip, _port);
                    if (await Task.WhenAny(connectTask, Task.Delay(200)) == connectTask && client.Connected)
                    {
                        lock (active)
                        {
                            active.Add(ip);
                        }
                    }
                }
            }
            catch
            {
                // ignore unreachable hosts
            }
        }
    }
}
