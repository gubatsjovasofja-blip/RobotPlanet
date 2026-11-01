
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RobotPlanet.Core
{
    public class CalculationRunner
    {
        // Ühe roboti arvutus
        private CalculationResult CalculateOne(
            RobotCalculation robot,
            int amount,
            CancellationToken token)
        {
            CalculationResult result = new CalculationResult();

            result.RobotName = robot.Name;
            result.RobotType = robot.Type;

            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                result.Result = robot.Calculate(amount, token);
                result.Status = "Valmis";
            }
            catch (OperationCanceledException)
            {
                result.Status = "Tühistatud";
            }
            catch (Exception ex)
            {
                result.Status = "Viga";
                result.Error = ex.Message;
            }

            watch.Stop();
            result.TimeMs = watch.ElapsedMilliseconds;

            return result;
        }

        // Järjestikune arvutamine
        public List<CalculationResult> RunSequential(
            List<RobotCalculation> robots,
            int amount,
            CancellationToken token)
        {
            List<CalculationResult> results =
                new List<CalculationResult>();

            foreach (RobotCalculation robot in robots)
            {
                CalculationResult result =
                    CalculateOne(robot, amount, token);

                results.Add(result);
            }

            return results;
        }

        // Paralleelne arvutamine
        public async Task<List<CalculationResult>> RunParallel(
            List<RobotCalculation> robots,
            int amount,
            CancellationToken token)
        {
            List<Task<CalculationResult>> tasks =
                new List<Task<CalculationResult>>();

            foreach (RobotCalculation robot in robots)
            {
                Task<CalculationResult> task = Task.Run(
                    () => CalculateOne(robot, amount, token));

                tasks.Add(task);
            }

            CalculationResult[] results =
                await Task.WhenAll(tasks);

            return new List<CalculationResult>(results);
        }
    }
}
