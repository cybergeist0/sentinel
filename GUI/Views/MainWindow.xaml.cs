using System.Windows;
using Microsoft.Win32;
using GUI.ViewModels;
using GUI.Models;
using GUI.Services;
using System.Collections.Generic;

namespace GUI.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;
        private readonly ExportService _export = new();
        private readonly ConfigurationService _config = new();

        public MainWindow()
        {
            InitializeComponent();
            _vm = (MainViewModel)DataContext;
            ShowSettings();
        }

        private async void OnScanClick(object sender, RoutedEventArgs e)
        {
            txtStatus.Text = "Scanning...";
            txtBottomStatus.Text = "Refreshing...";
            await _vm.RefreshAsync();
            txtStatus.Text = "Idle";
            txtBottomStatus.Text = "Done";
        }

        private void OnExportClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "server_audit.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                _export.ExportCsv(dialog.FileName, _vm.Servers as IEnumerable<ServerInfo>);
                txtBottomStatus.Text = $"Exported to {dialog.FileName}";
            }
        }

        private void ShowSettings()
        {
            int interval = _config.ScanIntervalMinutes;
            string path = _config.DefaultExportPath;
            bool autoScan = _config.EnableAutoScan;

            txtBottomStatus.Text = $"Config: Every {interval} min | Default Export: {path} | AutoScan: {(autoScan ? "On" : "Off")}";
        }
    }
}
