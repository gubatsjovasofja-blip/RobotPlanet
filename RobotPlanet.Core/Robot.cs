
using System;

namespace RobotPlanet.Core
{
    public abstract class Robot
    {
        public string Name { get; }
        public int Energy { get; private set; }

        public Robot(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid robot name.");
            }

            Name = name;
            Energy = 100;
        }

        protected void UseEnergy(int amount)
        {
            if (amount <= 0 || amount > Energy)
            {
                throw new ArgumentException("Not enough energy.");
            }

            Energy -= amount;
        }

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

        public abstract string NormalAction();

        public abstract string CrazyAction();

        public override string ToString()
        {
            return Name;
        }
    }
}
