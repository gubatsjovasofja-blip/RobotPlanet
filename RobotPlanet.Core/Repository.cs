
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotPlanet.Core
{
    public class Repository<T> : IRepository<T> where T : Robot
    {
        private List<T> items = new List<T>();

        // Три события для роботов
        public event Action<T>? RobotAdded;
        public event Action<T>? RobotRemoved;
        public event Action<T>? RobotActed;

        // Сообщения для журнала
        public event Action<string>? MessageCreated;

        // Добавляю робота и подписываю его на события
        public void Add(T item)
        {
            if (items.Contains(item))
                return;

            // Старые роботы приветствуют нового
            RobotAdded?.Invoke(item);

            items.Add(item);

            item.MessageCreated += AddMessage;
            RobotAdded += item.OnRobotAdded;
            RobotRemoved += item.OnRobotRemoved;
            RobotActed += item.OnRobotActed;
        }

        // Удаляю робота и его подписки
        public void Remove(T item)
        {
            if (!items.Contains(item))
                return;

            items.Remove(item);

            RobotAdded -= item.OnRobotAdded;
            RobotRemoved -= item.OnRobotRemoved;
            RobotActed -= item.OnRobotActed;
            item.MessageCreated -= AddMessage;

            // Остальные роботы прощаются
            RobotRemoved?.Invoke(item);
        }

        // Робот делает CrazyAction, остальные реагируют
        public void CrazyAction(T item)
        {
            if (!items.Contains(item))
                return;

            string result = item.CrazyAction();
            MessageCreated?.Invoke(result);

            RobotActed?.Invoke(item);
        }

        // Передаю сообщения в журнал
        private void AddMessage(string message)
        {
            MessageCreated?.Invoke(message);
        }

        public IEnumerable<T> GetAll()
        {
            return items.ToList();
        }

        public T? Find(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        // Поиск по имени
        public IEnumerable<T> Search(string name)
        {
            return items.Where(x =>
                x.Name.Contains(name,
                StringComparison.OrdinalIgnoreCase));
        }

        // Сортировка по имени
        public IEnumerable<T> SortByName()
        {
            return items.OrderBy(x => x.Name);
        }

        // Сортировка по энергии
        public IEnumerable<T> SortByEnergy()
        {
            return items.OrderByDescending(x => x.Energy);
        }

        public int Count()
        {
            return items.Count;
        }
    }
}
