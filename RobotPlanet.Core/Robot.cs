
using System;

namespace RobotPlanet.Core
{
    public abstract class Robot
    {
        public Guid Id { get; private set; }
        public string Name { get; }
        public int Energy { get; private set; }
        public string GroupName { get; private set; }

        public event Action<string>? MessageCreated;

        public string RobotType
        {
            get { return GetType().Name; }
        }

        public Robot(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Invalid robot name.");

            Id = Guid.NewGuid();
            Name = name;
            Energy = 100;
            GroupName = "Default";
        }

        // Taastan roboti andmed salvestatud failist
        public void RestoreState(Guid id, int energy, string groupName)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid robot ID.");

            if (energy < 0 || energy > 100)
                throw new ArgumentException("Invalid energy.");

            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Invalid group.");

            Id = id;
            Energy = energy;
            GroupName = groupName;
        }

        // Muudan roboti gruppi
        public void ChangeGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Invalid group.");

            GroupName = groupName;
        }

        // Vähendan roboti energiat
        protected void UseEnergy(int amount)
        {
            if (amount <= 0 || amount > Energy)
                throw new ArgumentException("Not enough energy.");

            Energy -= amount;
        }

        // Lisan robotile energiat
        protected void AddEnergy(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Invalid energy.");

            Energy += amount;

            if (Energy > 100)
                Energy = 100;
        }

        // Tervitan uut robotit
        public void OnRobotAdded(Robot other)
        {
            if (this != other)
                MessageCreated?.Invoke(
                    Name + " welcomes " + other.Name);
        }

        // Jätan eemaldatud robotiga hüvasti
        public void OnRobotRemoved(Robot other)
        {
            if (this != other)
                MessageCreated?.Invoke(
                    Name + " says goodbye to " + other.Name);
        }

        // Reageerin teise roboti tegevusele
        public void OnRobotActed(Robot other)
        {
            if (this != other)
                MessageCreated?.Invoke(React(other));
        }

        // Reaktsioon vähendab energiat 5 võrra
        public string React(Robot other)
        {
            Energy -= 5;

            if (Energy < 0)
                Energy = 0;

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
