
using System;
using System.Threading;

namespace RobotPlanet.Core
{
    public class RobotCalculation
    {
        public string Name { get; }
        public string Type { get; }
        public int Energy { get; }

        // Salvestan roboti andmed
        public RobotCalculation(Robot robot)
        {
            Name = robot.Name;
            Type = robot.RobotType;
            Energy = robot.Energy;
        }

        // Robot teeb arvutusi
        public double Calculate(int amount, CancellationToken token)
        {
            // Kontrollin, kas robotil on energiat
            if (Energy <= 0)
            {
                throw new InvalidOperationException(
                    "Robotil ei ole piisavalt energiat.");
            }

            double result = 0;

            for (int i = 0; i < amount; i++)
            {
                // Kontrollin tühistamist
                token.ThrowIfCancellationRequested();

                double number = i + Energy + 1;
                result += Math.Sqrt(number) * Math.Sin(number);
            }

            return result;
        }
    }
}
