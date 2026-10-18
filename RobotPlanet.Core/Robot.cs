
using System;

namespace RobotPlanet.Core
{
    public abstract class Robot
    {
        public string Name { get; }
        public int Energy { get; private set; }

        public event Action<string>? MessageCreated;

        public string RobotType
        {
            get { return GetType().Name; }
        }

        public Robot(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid robot name.");
            }

            Name = name;
            Energy = 100;
        }

        // Уменьшаю энергию робота
        protected void UseEnergy(int amount)
        {
            if (amount <= 0 || amount > Energy)
            {
                throw new ArgumentException("Not enough energy.");
            }

            Energy -= amount;
        }

        // Добавляю энергию роботу
        protected void AddEnergy(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Invalid energy.");
            }

            Energy += amount;

            if (Energy > 100)
            {
                Energy = 100;
            }
        }

        // Приветствие нового робота
        public void OnRobotAdded(Robot other)
        {
            if (this != other)
            {
                MessageCreated?.Invoke(
                    Name + " welcomes " + other.Name);
            }
        }

        // Прощание с удаленным роботом
        public void OnRobotRemoved(Robot other)
        {
            if (this != other)
            {
                MessageCreated?.Invoke(
                    Name + " says goodbye to " + other.Name);
            }
        }

        // Реакция на CrazyAction другого робота
        public void OnRobotActed(Robot other)
        {
            if (this != other)
            {
                MessageCreated?.Invoke(React(other));
            }
        }

        // При реакции робот теряет 5 энергии
        public string React(Robot other)
        {
            Energy -= 5;

            if (Energy < 0)
            {
                Energy = 0;
            }

            return Name + " reacts to " + other.Name
                + " (-5 energy)";
        }

        public abstract string NormalAction();

        public abstract string CrazyAction();

        public override string ToString()
        {
            return Name;
        }
    }
}
