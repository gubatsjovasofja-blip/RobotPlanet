

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RobotPlanet.Core;

namespace RobotPlanet
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private RobotUniverse universe = new RobotUniverse();
        private readonly IWorldStorage storage;

        private Robot? selectedRobot;
        private string robotName = "";
        private string newGroupName = "";
        private string searchText = "";
        private string sortMode = "";
        private string selectedRepository = "Kõik robotid";

        public ObservableCollection<Robot> Robots { get; } = new();
        public ObservableCollection<string> Logs { get; } = new();
        public ObservableCollection<string> GroupSummary { get; } = new();

        public ObservableCollection<string> RepositoryNames { get; } = new()
        {
            "Kõik robotid", "CleanerBot", "ExplorerBot",
            "RepairBot", "DanceBot", "ChefBot",
            "GuardBot", "FlyingBot"
        };

        public string SelectedRepository
        {
            get { return selectedRepository; }
            set
            {
                selectedRepository = value;
                Notify("SelectedRepository");
                RefreshRobots();
            }
        }

        public string RobotName
        {
            get { return robotName; }
            set
            {
                robotName = value;
                Notify("RobotName");
                AddCleanerCommand?.Update();
                AddExplorerCommand?.Update();
                AddRepairCommand?.Update();
                AddDanceCommand?.Update();
                AddChefCommand?.Update();
                AddGuardCommand?.Update();
                AddFlyingCommand?.Update();
            }
        }

        public string NewGroupName
        {
            get { return newGroupName; }
            set
            {
                newGroupName = value;
                Notify("NewGroupName");
                ChangeGroupCommand?.Update();
            }
        }

        public string SelectedGroup => SelectedRobot?.GroupName ?? "";

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                Notify("SearchText");
                RefreshRobots();
            }
        }

        public Robot? SelectedRobot
        {
            get { return selectedRobot; }
            set
            {
                selectedRobot = value;
                Notify("SelectedRobot");
                Notify("SelectedName");
                Notify("SelectedGroup");
                NewGroupName = selectedRobot?.GroupName ?? "";
                Notify("Energy");
                Notify("EnergyText");
                UpdateCommands();
            }
        }

        public string SelectedName => SelectedRobot?.Name ?? "";
        public int Energy => SelectedRobot?.Energy ?? 0;
        public string EnergyText =>
            SelectedRobot == null ? "" : Energy + " / 100";

        public bool IsGrouped => sortMode == "Group";

        public RelayCommand AddCleanerCommand { get; }
        public RelayCommand AddExplorerCommand { get; }
        public RelayCommand AddRepairCommand { get; }
        public RelayCommand AddDanceCommand { get; }
        public RelayCommand AddChefCommand { get; }
        public RelayCommand AddGuardCommand { get; }
        public RelayCommand AddFlyingCommand { get; }

        public RelayCommand ChangeGroupCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand NormalCommand { get; }
        public RelayCommand CrazyCommand { get; }
        public RelayCommand ChargeCommand { get; }
        public RelayCommand ScanCommand { get; }

        public RelayCommand SortNameCommand { get; }
        public RelayCommand SortEnergyCommand { get; }
        public RelayCommand GroupCommand { get; }
        public RelayCommand ShowAllCommand { get; }

        // Saan salvestamise teenuse konstruktori kaudu
        public MainViewModel(IWorldStorage storage)
        {
            this.storage = storage ??
                throw new ArgumentNullException(nameof(storage));

            ConnectLogs(universe);

            AddCleanerCommand = new RelayCommand(
                () => AddRobot("Cleaner"), CanAddRobot);
            AddExplorerCommand = new RelayCommand(
                () => AddRobot("Explorer"), CanAddRobot);
            AddRepairCommand = new RelayCommand(
                () => AddRobot("Repair"), CanAddRobot);
            AddDanceCommand = new RelayCommand(
                () => AddRobot("Dance"), CanAddRobot);
            AddChefCommand = new RelayCommand(
                () => AddRobot("Chef"), CanAddRobot);
            AddGuardCommand = new RelayCommand(
                () => AddRobot("Guard"), CanAddRobot);
            AddFlyingCommand = new RelayCommand(
                () => AddRobot("Flying"), CanAddRobot);

            ChangeGroupCommand = new RelayCommand(
                ChangeGroup, () => SelectedRobot != null &&
                    !string.IsNullOrWhiteSpace(NewGroupName));

            RemoveCommand = new RelayCommand(
                RemoveRobot, () => SelectedRobot != null);
            NormalCommand = new RelayCommand(
                NormalAction, () => SelectedRobot != null);
            CrazyCommand = new RelayCommand(
                CrazyAction, () => SelectedRobot != null);
            ChargeCommand = new RelayCommand(
                Charge, () => SelectedRobot is IChargeable);
            ScanCommand = new RelayCommand(
                Scan, () => SelectedRobot is IScannable);

            SortNameCommand = new RelayCommand(
                () => SortRobots("Name"));
            SortEnergyCommand = new RelayCommand(
                () => SortRobots("Energy"));
            GroupCommand = new RelayCommand(
                () => SortRobots("Group"));
            ShowAllCommand = new RelayCommand(ShowAll);

            // Alguses loodud robotid
            universe.Add(new CleanerBot("Cleaner 1"));
            universe.Add(new ExplorerBot("Explorer 1"));
            universe.Add(new RepairBot("Repair 1"));
            universe.Add(new ChefBot("Chef 1"));
            universe.Add(new GuardBot("Guard 1"));
            universe.Add(new FlyingBot("Flying 1"));

            RefreshRobots();
        }

        // Ühendan hoidlate sõnumid logiga
        private void ConnectLogs(RobotUniverse world)
        {
            world.Cleaners.MessageCreated += AddLog;
            world.Explorers.MessageCreated += AddLog;
            world.Repairers.MessageCreated += AddLog;
            world.Dancers.MessageCreated += AddLog;
            world.Chefs.MessageCreated += AddLog;
            world.Guards.MessageCreated += AddLog;
            world.Flyers.MessageCreated += AddLog;
        }

        // Eemaldan vana maailma logiühendused
        private void DisconnectLogs(RobotUniverse world)
        {
            world.Cleaners.MessageCreated -= AddLog;
            world.Explorers.MessageCreated -= AddLog;
            world.Repairers.MessageCreated -= AddLog;
            world.Dancers.MessageCreated -= AddLog;
            world.Chefs.MessageCreated -= AddLog;
            world.Guards.MessageCreated -= AddLog;
            world.Flyers.MessageCreated -= AddLog;
        }

        // Salvestan maailma valitud teenusega
        public async Task SaveWithStorageAsync(string path, IWorldStorage selectedStorage)
        {
            WorldDto data = WorldMapper.ToDto(universe);
            await selectedStorage.SaveAsync(data, path);
            AddLog("Maailm salvestatud: " + path);
        }

        // Laen maailma valitud teenusega
        public async Task LoadWithStorageAsync(string path, IWorldStorage selectedStorage)
        {
            WorldDto data = await selectedStorage.LoadAsync(path);
            RobotUniverse newWorld = WorldMapper.FromDto(data);

            // Vahetan maailma alles pärast edukat kontrolli
            DisconnectLogs(universe);
            universe = newWorld;
            ConnectLogs(universe);

            sortMode = "";
            searchText = "";
            selectedRepository = "Kõik robotid";
            Notify("SearchText");
            Notify("SelectedRepository");
            RefreshRobots();
            AddLog("Maailm laaditud: " + path);
        }

        // Vaikimisi kasutan App.xaml.cs kaudu saadud teenust
        public Task SaveWorldAsync(string path)
        {
            return SaveWithStorageAsync(path, storage);
        }

        public Task LoadWorldAsync(string path)
        {
            return LoadWithStorageAsync(path, storage);
        }

        private bool CanAddRobot()
        {
            return !string.IsNullOrWhiteSpace(RobotName);
        }

        private void AddRobot(string type)
        {
            if (!CanAddRobot())
            {
                MessageBox.Show(
                    RobotPlanet.WpfApp.Resources.InvalidRobotName);
                return;
            }

            Robot robot;

            if (type == "Cleaner")
                robot = new CleanerBot(RobotName);
            else if (type == "Explorer")
                robot = new ExplorerBot(RobotName);
            else if (type == "Repair")
                robot = new RepairBot(RobotName);
            else if (type == "Dance")
                robot = new DanceBot(RobotName);
            else if (type == "Chef")
                robot = new ChefBot(RobotName);
            else if (type == "Guard")
                robot = new GuardBot(RobotName);
            else if (type == "Flying")
                robot = new FlyingBot(RobotName);
            else
                return;

            universe.Add(robot);
            RobotName = "";
            RefreshRobots();
        }

        // Muudan valitud roboti rühma
        private void ChangeGroup()
        {
            if (SelectedRobot == null || string.IsNullOrWhiteSpace(NewGroupName))
                return;

            string groupName = NewGroupName.Trim();
            if (groupName.Length > 100)
            {
                MessageBox.Show("Rühma nimi võib olla kuni 100 tähemärki.");
                return;
            }

            SelectedRobot.ChangeGroup(groupName);
            Notify("SelectedGroup");
            AddLog(SelectedRobot.Name + " uus rühm: " + groupName);
            RefreshRobots();
        }

        private void RemoveRobot()
        {
            if (SelectedRobot == null)
                return;

            universe.Remove(SelectedRobot);
            AddLog(RobotPlanet.WpfApp.Resources.RobotRemoved);
            RefreshRobots();
        }

        private void SortRobots(string mode)
        {
            sortMode = mode;
            RefreshRobots();
        }

        private void ShowAll()
        {
            sortMode = "";
            SearchText = "";
            SelectedRepository = "Kõik robotid";
            RefreshRobots();
        }

        private void RefreshRobots()
        {
            Robot? oldRobot = SelectedRobot;
            var result = universe.GetAllRobots();

            if (SelectedRepository != "Kõik robotid")
                result = result.Where(robot =>
                    robot.RobotType == SelectedRepository);

            if (!string.IsNullOrWhiteSpace(SearchText))
                result = result.Where(robot =>
                    robot.Name.Contains(SearchText,
                        StringComparison.OrdinalIgnoreCase));

            if (sortMode == "Name")
                result = result.OrderBy(robot => robot.Name);
            else if (sortMode == "Energy")
                result = result.OrderByDescending(robot => robot.Energy);
            else if (sortMode == "Group")
                result = result.OrderBy(robot => robot.GroupName)
                    .ThenBy(robot => robot.Name);

            Robots.Clear();

            foreach (Robot robot in result.ToList())
                Robots.Add(robot);

            GroupSummary.Clear();

            if (sortMode == "Group")
            {
                foreach (var group in Robots.GroupBy(
                    robot => robot.GroupName))
                {
                    GroupSummary.Add(
                        group.Key + ": " + group.Count());
                }
            }

            if (oldRobot != null && Robots.Contains(oldRobot))
                SelectedRobot = oldRobot;
            else
                SelectedRobot = Robots.FirstOrDefault();

            Notify("IsGrouped");
        }

        private void NormalAction()
        {
            if (SelectedRobot == null)
                return;

            try
            {
                AddLog(SelectedRobot.NormalAction());
                UpdateEnergy();
            }
            catch (ArgumentException)
            {
                ShowEnergyError();
            }
        }

        private void CrazyAction()
        {
            if (SelectedRobot == null)
                return;

            try
            {
                universe.CrazyAction(SelectedRobot);
                UpdateEnergy();
            }
            catch (ArgumentException)
            {
                ShowEnergyError();
            }
        }

        private void Charge()
        {
            if (SelectedRobot is IChargeable robot)
            {
                AddLog(robot.Charge());
                UpdateEnergy();
            }
        }

        private void Scan()
        {
            if (SelectedRobot is IScannable robot)
            {
                try
                {
                    AddLog(robot.Scan());
                    UpdateEnergy();
                }
                catch (ArgumentException)
                {
                    ShowEnergyError();
                }
            }
        }

        private void ShowEnergyError()
        {
            MessageBox.Show(
                RobotPlanet.WpfApp.Resources.NotEnoughEnergy);
        }

        private void UpdateEnergy()
        {
            Notify("Energy");
            Notify("EnergyText");

            if (sortMode == "Energy")
                RefreshRobots();
        }

        private void UpdateCommands()
        {
            ChangeGroupCommand?.Update();
            RemoveCommand?.Update();
            NormalCommand?.Update();
            CrazyCommand?.Update();
            ChargeCommand?.Update();
            ScanCommand?.Update();
        }

        private void AddLog(string message)
        {
            Logs.Add(message);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Notify(string name)
        {
            PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(name));
        }
    }
}
