
using System.Windows;

namespace RobotPlanet
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            Title = RobotPlanet.WpfApp.Resources.WindowTitle;

            RobotsTitleText.Text =
                RobotPlanet.WpfApp.Resources.RobotsTitle;

            SelectedRobotTitleText.Text =
                RobotPlanet.WpfApp.Resources.SelectedRobot;

            EnergyTitleText.Text =
                RobotPlanet.WpfApp.Resources.EnergyTitle;

            AddCleanerButton.Content =
                RobotPlanet.WpfApp.Resources.AddCleanerButton;

            AddExplorerButton.Content =
                RobotPlanet.WpfApp.Resources.AddExplorerButton;

            AddRepairButton.Content =
                RobotPlanet.WpfApp.Resources.AddRepairButton;

            AddDanceButton.Content =
                RobotPlanet.WpfApp.Resources.AddDanceButton;

            RemoveButton.Content =
                RobotPlanet.WpfApp.Resources.RemoveButton;

            NormalButton.Content =
                RobotPlanet.WpfApp.Resources.NormalButton;

            CrazyButton.Content =
                RobotPlanet.WpfApp.Resources.CrazyButton;

            ChargeButton.Content =
                RobotPlanet.WpfApp.Resources.ChargeButton;

            ScanButton.Content =
                RobotPlanet.WpfApp.Resources.ScanButton;

            LogTitleText.Text =
                RobotPlanet.WpfApp.Resources.LogTitle;
        }
    }
}
