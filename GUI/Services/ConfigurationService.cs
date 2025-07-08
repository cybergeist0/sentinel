using Microsoft.Extensions.Configuration;
using System.IO;

namespace GUI.Services
{
    public class ConfigurationService
    {
        private readonly IConfigurationRoot _config;

        public ConfigurationService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
        }

        public int ScanIntervalMinutes => int.TryParse(_config["ScanIntervalMinutes"], out int val) ? val : 30;
        public string DefaultExportPath => _config["DefaultExportPath"] ?? "C:\\Logs\\server_audit.csv";
        public bool EnableAutoScan => bool.TryParse(_config["EnableAutoScan"], out bool val) && val;
    }
}
