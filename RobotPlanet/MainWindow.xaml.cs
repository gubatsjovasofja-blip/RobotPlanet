
using System.Windows;
using System.Windows.Controls;

namespace RobotPlanet
{
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;
        private CalculationViewModel calculationViewModel;

        public MainWindow()
        {
            InitializeComponent();

            // Põhiprogramm
            mainViewModel = new MainViewModel();
            DataContext = mainViewModel;

            // Arvutuste osa
            calculationViewModel = new CalculationViewModel();
            CalculationsTab.DataContext = calculationViewModel;

            // Tekstid ressurssidest
            Title = RobotPlanet.WpfApp.Resources.WindowTitle;

            RobotsTitleText.Text =
                RobotPlanet.WpfApp.Resources.RobotsTitle;

            SelectedRobotTitleText.Text =
                RobotPlanet.WpfApp.Resources.SelectedRobot;

            EnergyTitleText.Text =
                RobotPlanet.WpfApp.Resources.EnergyTitle;

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

        // Lisan valitud tüüpi roboti
        private void AddRobotButton_Click(
            object sender, RoutedEventArgs e)
        {
            if (RobotTypeComboBox.SelectedItem is not ComboBoxItem item)
                return;

            string type = item.Content.ToString() ?? "";

            if (type == "CleanerBot")
                mainViewModel.AddCleanerCommand.Execute(null);

            else if (type == "ExplorerBot")
                mainViewModel.AddExplorerCommand.Execute(null);

            else if (type == "RepairBot")
                mainViewModel.AddRepairCommand.Execute(null);

            else if (type == "DanceBot")
                mainViewModel.AddDanceCommand.Execute(null);

            else if (type == "ChefBot")
                mainViewModel.AddChefCommand.Execute(null);

            else if (type == "GuardBot")
                mainViewModel.AddGuardCommand.Execute(null);

            else if (type == "FlyingBot")
                mainViewModel.AddFlyingCommand.Execute(null);
        }

        // Vahekaardi vahetamine
        private void Tabs_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != sender)
                return;

            if (mainViewModel == null || calculationViewModel == null)
                return;

            if (CalculationsTab.IsSelected &&
                !calculationViewModel.IsRunning)
            {
                calculationViewModel.SetRobots(mainViewModel.Robots);
            }
        }

        // Järjestikune arvutamine
        private async void SequentialButton_Click(
            object sender, RoutedEventArgs e)
        {
            await calculationViewModel.Start(false);
        }

        // Paralleelne arvutamine
        private async void ParallelButton_Click(
            object sender, RoutedEventArgs e)
        {
            await calculationViewModel.Start(true);
        }

        // Arvutamise tühistamine
        private void CancelButton_Click(
            object sender, RoutedEventArgs e)
        {
            calculationViewModel.Cancel();
        }
    }
}
