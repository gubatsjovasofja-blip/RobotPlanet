
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RobotPlanet.Core;

namespace RobotPlanet
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Robot> robots = new ObservableCollection<Robot>();

        public MainWindow()
        {
            InitializeComponent();

            Title = RobotPlanet.WpfApp.Resources.WindowTitle;
            RobotsTitleText.Text = RobotPlanet.WpfApp.Resources.RobotsTitle;
            SelectedRobotTitleText.Text = RobotPlanet.WpfApp.Resources.SelectedRobot;
            EnergyTitleText.Text = RobotPlanet.WpfApp.Resources.EnergyTitle;

            AddCleanerButton.Content = RobotPlanet.WpfApp.Resources.AddCleanerButton;
            AddExplorerButton.Content = RobotPlanet.WpfApp.Resources.AddExplorerButton;
            AddRepairButton.Content = RobotPlanet.WpfApp.Resources.AddRepairButton;
            AddDanceButton.Content = RobotPlanet.WpfApp.Resources.AddDanceButton;
            RemoveButton.Content = RobotPlanet.WpfApp.Resources.RemoveButton;

            NormalButton.Content = RobotPlanet.WpfApp.Resources.NormalButton;
            CrazyButton.Content = RobotPlanet.WpfApp.Resources.CrazyButton;
            ChargeButton.Content = RobotPlanet.WpfApp.Resources.ChargeButton;
            ScanButton.Content = RobotPlanet.WpfApp.Resources.ScanButton;
            LogTitleText.Text = RobotPlanet.WpfApp.Resources.LogTitle;

            RobotNameTextBox.ToolTip = RobotPlanet.WpfApp.Resources.RobotNameHint;

            RobotList.ItemsSource = robots;

            robots.Add(new CleanerBot("Cleaner 1"));
            robots.Add(new ExplorerBot("Explorer 1"));
            robots.Add(new RepairBot("Repair 1"));

            RobotList.SelectedIndex = 0;
        }

        private void AddCleaner_Click(object sender, RoutedEventArgs e)
        {
            string name = RobotNameTextBox.Text.Trim();

            if (name == "")
            {
                MessageBox.Show(RobotPlanet.WpfApp.Resources.InvalidRobotName);
                return;
            }

            robots.Add(new CleanerBot(name));
            RobotNameTextBox.Clear();
        }

        private void AddExplorer_Click(object sender, RoutedEventArgs e)
        {
            string name = RobotNameTextBox.Text.Trim();

            if (name == "")
            {
                MessageBox.Show(RobotPlanet.WpfApp.Resources.InvalidRobotName);
                return;
            }

            robots.Add(new ExplorerBot(name));
            RobotNameTextBox.Clear();
        }

        private void AddRepair_Click(object sender, RoutedEventArgs e)
        {
            string name = RobotNameTextBox.Text.Trim();

            if (name == "")
            {
                MessageBox.Show(RobotPlanet.WpfApp.Resources.InvalidRobotName);
                return;
            }

            robots.Add(new RepairBot(name));
            RobotNameTextBox.Clear();
        }



        private void AddDance_Click(object sender, RoutedEventArgs e)
        {
            string name = RobotNameTextBox.Text.Trim();

            if (name == "")
            {
                MessageBox.Show(RobotPlanet.WpfApp.Resources.InvalidRobotName);
                return;
            }

            robots.Add(new DanceBot(name));
            RobotNameTextBox.Clear();
        }



        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (RobotList.SelectedItem is Robot robot)
            {
                robots.Remove(robot);
                LogList.Items.Add(RobotPlanet.WpfApp.Resources.RobotRemoved);
            }
            else
            {
                MessageBox.Show(RobotPlanet.WpfApp.Resources.SelectRobot);
            }
        }

        private void RobotList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RobotList.SelectedItem is Robot robot)
            {
                SelectedRobotText.Text = robot.Name;
                UpdateEnergy();

                NormalButton.IsEnabled = true;
                CrazyButton.IsEnabled = true;
                ChargeButton.IsEnabled = robot is IChargeable;
                ScanButton.IsEnabled = robot is IScannable;
            }
            else
            {
                SelectedRobotText.Text = "";
                EnergyText.Text = "";
                EnergyBar.Value = 0;

                NormalButton.IsEnabled = false;
                CrazyButton.IsEnabled = false;
                ChargeButton.IsEnabled = false;
                ScanButton.IsEnabled = false;
            }
        }

        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            if (RobotList.SelectedItem is Robot robot)
            {
                try
                {
                    LogList.Items.Add(robot.NormalAction());
                    UpdateEnergy();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(RobotPlanet.WpfApp.Resources.NotEnoughEnergy);
                }
            }
        }

        private void Crazy_Click(object sender, RoutedEventArgs e)
        {
            if (RobotList.SelectedItem is Robot robot)
            {
                try
                {
                    LogList.Items.Add(robot.CrazyAction());
                    UpdateEnergy();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(RobotPlanet.WpfApp.Resources.NotEnoughEnergy);
                }
            }
        }

        private void Charge_Click(object sender, RoutedEventArgs e)
        {
            if (RobotList.SelectedItem is IChargeable robot)
            {
                LogList.Items.Add(robot.Charge());
                UpdateEnergy();
            }
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            if (RobotList.SelectedItem is IScannable robot)
            {
                try
                {
                    LogList.Items.Add(robot.Scan());
                    UpdateEnergy();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(RobotPlanet.WpfApp.Resources.NotEnoughEnergy);
                }
            }
        }

        private void UpdateEnergy()
        {
            if (RobotList.SelectedItem is Robot robot)
            {
                EnergyBar.Value = robot.Energy;
                EnergyText.Text = robot.Energy + " / 100";
            }
        }
    }
}
