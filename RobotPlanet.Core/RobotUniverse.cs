
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotPlanet.Core
{
    public class RobotUniverse
    {
        // У каждого типа роботов свой репозиторий
        public Repository<CleanerBot> Cleaners { get; } = new();
        public Repository<ExplorerBot> Explorers { get; } = new();
        public Repository<RepairBot> Repairers { get; } = new();
        public Repository<DanceBot> Dancers { get; } = new();

        // Добавляю робота в его репозиторий
        public void Add(Robot robot)
        {
            if (robot is CleanerBot cleaner)
                Cleaners.Add(cleaner);
            else if (robot is ExplorerBot explorer)
                Explorers.Add(explorer);
            else if (robot is RepairBot repair)
                Repairers.Add(repair);
            else if (robot is DanceBot dancer)
                Dancers.Add(dancer);
        }

        // Удаляю робота из его репозитория
        public void Remove(Robot robot)
        {
            if (robot is CleanerBot cleaner)
                Cleaners.Remove(cleaner);
            else if (robot is ExplorerBot explorer)
                Explorers.Remove(explorer);
            else if (robot is RepairBot repair)
                Repairers.Remove(repair);
            else if (robot is DanceBot dancer)
                Dancers.Remove(dancer);
        }

        // Выполняю CrazyAction в нужном репозитории
        public void CrazyAction(Robot robot)
        {
            if (robot is CleanerBot cleaner)
                Cleaners.CrazyAction(cleaner);
            else if (robot is ExplorerBot explorer)
                Explorers.CrazyAction(explorer);
            else if (robot is RepairBot repair)
                Repairers.CrazyAction(repair);
            else if (robot is DanceBot dancer)
                Dancers.CrazyAction(dancer);
        }

        // Собираю всех роботов для отображения
        public IEnumerable<Robot> GetAllRobots()
        {
            return Cleaners.GetAll().Cast<Robot>()
                .Concat(Explorers.GetAll())
                .Concat(Repairers.GetAll())
                .Concat(Dancers.GetAll());
        }

        // Ищу роботов по имени
        public IEnumerable<Robot> Search(string name)
        {
            return GetAllRobots().Where(robot =>
                robot.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        // Сортирую роботов по имени
        public IEnumerable<Robot> SortByName()
        {
            return GetAllRobots().OrderBy(robot => robot.Name);
        }

        // Сортирую роботов по энергии
        public IEnumerable<Robot> SortByEnergy()
        {
            return GetAllRobots().OrderByDescending(robot => robot.Energy);
        }

        // Группирую роботов по типу
        public IEnumerable<IGrouping<string, Robot>> GroupByType()
        {
            return GetAllRobots().GroupBy(robot => robot.RobotType);
        }
    }
}
