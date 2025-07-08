using System.Windows;
using GUI.Converters;

namespace GUI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.Resources.Add("MaintStatusConverter", new BoolToMaintenanceStatusConverter());

            base.OnStartup(e);
        }
    }
}
