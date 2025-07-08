using GUI.Models;
using GUI.Services;
using System.Collections.ObjectModel;

namespace GUI.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<ServerInfo> Servers { get; } = new();

        public MainViewModel()
        {
            Task.Run(() => RefreshAsync());
        }

        public async Task RefreshAsync()
        {
            Servers.Clear();
            var list = await NetworkScanner.ScanAsync("192.168.1", 5005);
            foreach (var s in list)
                Servers.Add(s);

            var db = new SQLiteService();
            foreach (var s in list)
                db.Upsert(s);
        }
    }
}
