using System;
using System.Threading.Tasks;

namespace Scanner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var scanner = new ScannerService("192.168.1", 5005);
            var agents = await scanner.DiscoverAgentsAsync();
            Console.WriteLine("Agents found:");
            foreach (var ip in agents)
                Console.WriteLine($" - {ip}");
        }
    }
}
