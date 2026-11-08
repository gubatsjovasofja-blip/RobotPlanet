
using System.Windows;
using RobotPlanet.Core;

namespace RobotPlanet
{
    public partial class App : Application
    {
        // Valin programmi käivitamisel salvestamise viisi
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IWorldStorage storage = new JsonWorldStorage();

            // Annan salvestamise teenuse peaaknale
            MainWindow window = new MainWindow(storage);

            window.Show();
        }
    }
}
