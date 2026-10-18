
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotPlanet.Core
{
    public class RobotUniverse
    {
        // Отдельный список для каждого типа роботов
        public Repository<CleanerBot> Cleaners { get; } = new();
        public Repository<ExplorerBot> Explorers { get; } = new();
        public Repository<RepairBot> Repairers { get; } = new();
        public Repository<DanceBot> Dancers { get; } = new();

        // Добавляю нового робота
        public void Add(Robot robot)
        {
            // Проверяю, что такого робота еще нет
            if (GetAllRobots().Contains(robot))
                return;

            // Роботы других типов приветствуют нового
            foreach (Robot other in GetAllRobots())
            {
                if (other.GetType() != robot.GetType())
                    other.OnRobotAdded(robot);
            }

            // Добавляю робота в нужный список
            if (robot is CleanerBot cleaner)
                Cleaners.Add(cleaner);
            else if (robot is ExplorerBot explorer)
                Explorers.Add(explorer);
            else if (robot is RepairBot repair)
                Repairers.Add(repair);
            else if (robot is DanceBot dancer)
                Dancers.Add(dancer);
        }

        // Удаляю выбранного робота
        public void Remove(Robot robot)
        {
            // Проверяю, есть ли такой робот
            if (!GetAllRobots().Contains(robot))
                return;

            // Удаляю робота из его списка
            if (robot is CleanerBot cleaner)
                Cleaners.Remove(cleaner);
            else if (robot is ExplorerBot explorer)
                Explorers.Remove(explorer);
            else if (robot is RepairBot repair)
                Repairers.Remove(repair);
            else if (robot is DanceBot dancer)
                Dancers.Remove(dancer);

            // Роботы других типов прощаются с ним
            foreach (Robot other in GetAllRobots())
            {
                if (other.GetType() != robot.GetType())
                    other.OnRobotRemoved(robot);
            }
        }

        // Выполняю CrazyAction
        public void CrazyAction(Robot robot)
        {
            // Проверяю, есть ли такой робот
            if (!GetAllRobots().Contains(robot))
                return;

            // Запускаю действие выбранного робота
            if (robot is CleanerBot cleaner)
                Cleaners.CrazyAction(cleaner);
            else if (robot is ExplorerBot explorer)
                Explorers.CrazyAction(explorer);
            else if (robot is RepairBot repair)
                Repairers.CrazyAction(repair);
            else if (robot is DanceBot dancer)
                Dancers.CrazyAction(dancer);

            // Роботы других типов реагируют
            foreach (Robot other in GetAllRobots())
            {
                if (other.GetType() != robot.GetType())
                    other.OnRobotActed(robot);
            }
        }

        // Собираю всех роботов в один список
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
