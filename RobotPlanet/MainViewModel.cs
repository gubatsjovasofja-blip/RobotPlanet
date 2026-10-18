
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using RobotPlanet.Core;

namespace RobotPlanet
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private RobotUniverse universe = new RobotUniverse();

        private Robot? selectedRobot;
        private string robotName = "";
        private string searchText = "";
        private string sortMode = "";
        private string selectedRepository = "Kõik robotid";

        // Списки для окна
        public ObservableCollection<Robot> Robots { get; } = new();
        public ObservableCollection<string> Logs { get; } = new();
        public ObservableCollection<string> GroupSummary { get; } = new();

        // Названия репозиториев
        public ObservableCollection<string> RepositoryNames { get; } = new()
        {
            "Kõik robotid",
            "CleanerBot",
            "ExplorerBot",
            "RepairBot",
            "DanceBot"
        };

        // Выбранный репозиторий
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

        // Имя нового робота
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
            }
        }

        // Текст поиска
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

        // Выбранный робот
        public Robot? SelectedRobot
        {
            get { return selectedRobot; }
            set
            {
                selectedRobot = value;

                Notify("SelectedRobot");
                Notify("SelectedName");
                Notify("Energy");
                Notify("EnergyText");

                UpdateCommands();
            }
        }

        public string SelectedName
        {
            get { return SelectedRobot?.Name ?? ""; }
        }

        public int Energy
        {
            get { return SelectedRobot?.Energy ?? 0; }
        }

        public string EnergyText
        {
            get
            {
                return SelectedRobot == null
                    ? ""
                    : Energy + " / 100";
            }
        }

        public bool IsGrouped
        {
            get { return sortMode == "Group"; }
        }

        // Команды
        public RelayCommand AddCleanerCommand { get; }
        public RelayCommand AddExplorerCommand { get; }
        public RelayCommand AddRepairCommand { get; }
        public RelayCommand AddDanceCommand { get; }

        public RelayCommand RemoveCommand { get; }
        public RelayCommand NormalCommand { get; }
        public RelayCommand CrazyCommand { get; }
        public RelayCommand ChargeCommand { get; }
        public RelayCommand ScanCommand { get; }

        public RelayCommand SortNameCommand { get; }
        public RelayCommand SortEnergyCommand { get; }
        public RelayCommand GroupCommand { get; }
        public RelayCommand ShowAllCommand { get; }

        public MainViewModel()
        {
            // Получаю сообщения из репозиториев
            universe.Cleaners.MessageCreated += AddLog;
            universe.Explorers.MessageCreated += AddLog;
            universe.Repairers.MessageCreated += AddLog;
            universe.Dancers.MessageCreated += AddLog;

            // Команды добавления роботов
            AddCleanerCommand = new RelayCommand(
                () => AddRobot("Cleaner"), CanAddRobot);

            AddExplorerCommand = new RelayCommand(
                () => AddRobot("Explorer"), CanAddRobot);

            AddRepairCommand = new RelayCommand(
                () => AddRobot("Repair"), CanAddRobot);

            AddDanceCommand = new RelayCommand(
                () => AddRobot("Dance"), CanAddRobot);

            // Команды действий
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

            // Команды поиска, сортировки и группировки
            SortNameCommand = new RelayCommand(
                () => SortRobots("Name"));

            SortEnergyCommand = new RelayCommand(
                () => SortRobots("Energy"));

            GroupCommand = new RelayCommand(
                () => SortRobots("Group"));

            ShowAllCommand = new RelayCommand(ShowAll);

            // Начальные роботы
            universe.Add(new CleanerBot("Cleaner 1"));
            universe.Add(new ExplorerBot("Explorer 1"));
            universe.Add(new RepairBot("Repair 1"));

            RefreshRobots();
        }

        // Проверяю имя нового робота
        private bool CanAddRobot()
        {
            return !string.IsNullOrWhiteSpace(RobotName);
        }

        // Добавляю нового робота
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
            else
                robot = new DanceBot(RobotName);

            universe.Add(robot);

            RobotName = "";
            RefreshRobots();
        }

        // Удаляю выбранного робота
        private void RemoveRobot()
        {
            if (SelectedRobot == null)
                return;

            universe.Remove(SelectedRobot);

            AddLog(RobotPlanet.WpfApp.Resources.RobotRemoved);

            RefreshRobots();
        }

        // Выбираю сортировку или группировку
        private void SortRobots(string mode)
        {
            sortMode = mode;
            RefreshRobots();
        }

        // Показываю всех роботов
        private void ShowAll()
        {
            sortMode = "";
            SearchText = "";
            SelectedRepository = "Kõik robotid";

            RefreshRobots();
        }

        // Обновляю список роботов
        private void RefreshRobots()
        {
            Robot? oldRobot = SelectedRobot;

            // Получаю всех роботов
            var result = universe.GetAllRobots();

            // Фильтрую по выбранному репозиторию
            if (SelectedRepository != "Kõik robotid")
            {
                result = result.Where(robot =>
                    robot.RobotType == SelectedRepository);
            }

            // Ищу по имени
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                result = result.Where(robot =>
                    robot.Name.Contains(
                        SearchText,
                        StringComparison.OrdinalIgnoreCase));
            }

            // Сортирую или группирую
            if (sortMode == "Name")
            {
                result = result.OrderBy(robot => robot.Name);
            }
            else if (sortMode == "Energy")
            {
                result = result.OrderByDescending(robot => robot.Energy);
            }
            else if (sortMode == "Group")
            {
                result = result.OrderBy(robot => robot.RobotType);
            }

            // Обновляю отображаемый список
            Robots.Clear();

            foreach (Robot robot in result.ToList())
            {
                Robots.Add(robot);
            }

            // Обновляю количество роботов каждого типа
            GroupSummary.Clear();

            if (sortMode == "Group")
            {
                var groups = Robots.GroupBy(
                    robot => robot.RobotType);

                foreach (var group in groups)
                {
                    GroupSummary.Add(
                        group.Key + ": " + group.Count());
                }
            }

            // Сохраняю выбор, если робот ещё в списке
            if (oldRobot != null && Robots.Contains(oldRobot))
            {
                SelectedRobot = oldRobot;
            }
            else
            {
                SelectedRobot = Robots.FirstOrDefault();
            }

            Notify("IsGrouped");
        }

        // Обычное действие
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

        // CrazyAction и реакции роботов того же типа
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

        // Зарядка робота
        private void Charge()
        {
            if (SelectedRobot is IChargeable robot)
            {
                AddLog(robot.Charge());
                UpdateEnergy();
            }
        }

        // Сканирование
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

        // Сообщение об ошибке энергии
        private void ShowEnergyError()
        {
            MessageBox.Show(
                RobotPlanet.WpfApp.Resources.NotEnoughEnergy);
        }

        // Обновляю энергию после действия
        private void UpdateEnergy()
        {
            Notify("Energy");
            Notify("EnergyText");

            // При сортировке по энергии обновляю список
            if (sortMode == "Energy")
                RefreshRobots();
        }

        // Обновляю доступность кнопок
        private void UpdateCommands()
        {
            RemoveCommand?.Update();
            NormalCommand?.Update();
            CrazyCommand?.Update();
            ChargeCommand?.Update();
            ScanCommand?.Update();
        }

        // Добавляю сообщение в журнал
        private void AddLog(string message)
        {
            Logs.Add(message);
        }

        // Уведомляю интерфейс об изменениях
        public event PropertyChangedEventHandler? PropertyChanged;

        private void Notify(string name)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(name));
        }
    }
}
