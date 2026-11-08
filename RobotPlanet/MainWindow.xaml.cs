
using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using RobotPlanet.Core;

namespace RobotPlanet
{
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;
        private CalculationViewModel calculationViewModel;

        public MainWindow(IWorldStorage storage)
        {
            InitializeComponent();

            // Põhiprogramm
            mainViewModel = new MainViewModel(storage);
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

        // Salvestan maailma JSON-faili
        private async void SaveJsonButton_Click(
            object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "JSON fail (*.json)|*.json",
                DefaultExt = ".json",
                FileName = "robot_world.json"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                await mainViewModel.SaveWorldAsync(
                    dialog.FileName);

                MessageBox.Show(
                    "Maailm on salvestatud!",
                    "Robot Planet");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Salvestamine ebaõnnestus: " + ex.Message,
                    "Viga",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Laen maailma JSON-failist
        private async void LoadJsonButton_Click(
            object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "JSON fail (*.json)|*.json",
                DefaultExt = ".json"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                await mainViewModel.LoadWorldAsync(
                    dialog.FileName);

                MessageBox.Show(
                    "Maailm on laaditud!",
                    "Robot Planet");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Laadimine ebaõnnestus: " + ex.Message,
                    "Viga",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Salvestan maailma XML-faili
        private async void SaveXmlButton_Click(
            object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "XML fail (*.xml)|*.xml",
                DefaultExt = ".xml",
                FileName = "robot_world.xml"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                await mainViewModel.SaveWithStorageAsync(
                    dialog.FileName,
                    new XmlWorldStorage());

                MessageBox.Show(
                    "XML-fail on salvestatud!",
                    "Robot Planet");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "XML salvestamine ebaõnnestus: " + ex.Message,
                    "Viga",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Laen maailma XML-failist
        private async void LoadXmlButton_Click(
            object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "XML fail (*.xml)|*.xml",
                DefaultExt = ".xml"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                await mainViewModel.LoadWithStorageAsync(
                    dialog.FileName,
                    new XmlWorldStorage());

                MessageBox.Show(
                    "XML-fail on laaditud!",
                    "Robot Planet");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "XML laadimine ebaõnnestus: " + ex.Message,
                    "Viga",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Salvestan maailma LocalDB andmebaasi
        private async void SaveLocalDbButton_Click(
            object sender, RoutedEventArgs e)
        {
            try
            {
                IWorldStorage dbStorage =
                    new LocalDbWorldStorage();

                await mainViewModel.SaveWithStorageAsync(
                    "RobotPlanetDb",
                    dbStorage);

                MessageBox.Show(
                    "Maailm on LocalDB-sse salvestatud!",
                    "Robot Planet");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "LocalDB salvestamine ebaõnnestus: "
                    + ex.Message,
                    "Viga",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Laen maailma LocalDB andmebaasist
        private async void LoadLocalDbButton_Click(
            object sender, RoutedEventArgs e)
        {
            try
            {
                IWorldStorage dbStorage =
                    new LocalDbWorldStorage();

                await mainViewModel.LoadWithStorageAsync(
                    "RobotPlanetDb",
                    dbStorage);

                MessageBox.Show(
                    "Maailm on LocalDB-st laaditud!",
                    "Robot Planet");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "LocalDB laadimine ebaõnnestus: "
                    + ex.Message,
                    "Viga",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Lisan valitud tüüpi roboti
        private void AddRobotButton_Click(
            object sender, RoutedEventArgs e)
        {
            if (RobotTypeComboBox.SelectedItem
                is not ComboBoxItem item)
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

            if (mainViewModel == null ||
                calculationViewModel == null)
                return;

            if (CalculationsTab.IsSelected &&
                !calculationViewModel.IsRunning)
            {
                calculationViewModel.SetRobots(
                    mainViewModel.Robots);
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
