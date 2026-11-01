
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using RobotPlanet.Core;

namespace RobotPlanet
{
    public class CalculationViewModel : INotifyPropertyChanged
    {
        private CalculationRunner runner = new CalculationRunner();
        private CancellationTokenSource? cancellation;

        private int workAmount = 10000000;
        private bool isRunning = false;
        private string totalTime = "";
        private string message = "";
        private string comparison = "";

        // Samad andmed mõlema režiimi jaoks
        private List<RobotCalculation>? savedRobots;
        private int savedAmount;

        private long? sequentialTime;
        private long? parallelTime;

        public ObservableCollection<RobotChoice> Robots { get; } = new();
        public ObservableCollection<CalculationResult> Results { get; } = new();

        public int WorkAmount
        {
            get { return workAmount; }
            set
            {
                workAmount = value;
                Notify("WorkAmount");
            }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                isRunning = value;
                Notify("IsRunning");
                Notify("CanStart");
            }
        }

        public bool CanStart
        {
            get { return !IsRunning; }
        }

        public string TotalTime
        {
            get { return totalTime; }
            set
            {
                totalTime = value;
                Notify("TotalTime");
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                Notify("Message");
            }
        }

        public string Comparison
        {
            get { return comparison; }
            set
            {
                comparison = value;
                Notify("Comparison");
            }
        }

        // Robotite nimekiri
        public void SetRobots(IEnumerable<Robot> robots)
        {
            Robots.Clear();

            foreach (Robot robot in robots)
            {
                Robots.Add(new RobotChoice(robot));
            }

            // Uus robotite nimekiri tähendab uut võrdlust
            savedRobots = null;
            sequentialTime = null;
            parallelTime = null;
            Comparison = "";
        }

        // Arvutuste käivitamine
        public async Task Start(bool parallel)
        {
            if (IsRunning)
                return;

            List<RobotCalculation> selected = new();

            foreach (RobotChoice choice in Robots)
            {
                if (choice.IsSelected)
                {
                    selected.Add(new RobotCalculation(choice.Robot));
                }
            }

            if (selected.Count < 3)
            {
                Message = "Vali vähemalt 3 robotit.";
                return;
            }

            if (WorkAmount <= 0)
            {
                Message = "Töömaht peab olema suurem kui 0.";
                return;
            }

            // Kasutame võrdlemiseks sama hetktõmmist
            bool newInput = savedRobots == null ||
                            savedAmount != WorkAmount ||
                            savedRobots.Count != selected.Count;

            if (!newInput)
            {
                for (int i = 0; i < selected.Count; i++)
                {
                    if (savedRobots![i].Name != selected[i].Name ||
                        savedRobots[i].Type != selected[i].Type)
                    {
                        newInput = true;
                        break;
                    }
                }
            }

            if (newInput)
            {
                savedRobots = selected;
                savedAmount = WorkAmount;
                sequentialTime = null;
                parallelTime = null;
                Comparison = "";
            }

            IsRunning = true;
            Results.Clear();
            TotalTime = "";
            Message = "Arvutamine käib...";

            cancellation = new CancellationTokenSource();
            CancellationToken token = cancellation.Token;

            Stopwatch watch = Stopwatch.StartNew();
            bool completed = false;

            try
            {
                List<CalculationResult> results;

                if (parallel)
                {
                    results = await runner.RunParallel(
                        savedRobots!, savedAmount, token);
                }
                else
                {
                    results = await Task.Run(() =>
                        runner.RunSequential(
                            savedRobots!, savedAmount, token));
                }

                foreach (CalculationResult result in results)
                {
                    Results.Add(result);
                }

                completed = results.TrueForAll(
                    result => result.Status == "Valmis");

                Message = "Arvutamine lõpetatud.";
            }
            catch (Exception ex)
            {
                Message = "Viga: " + ex.Message;
            }
            finally
            {
                watch.Stop();

                long milliseconds = watch.ElapsedMilliseconds;

                TotalTime = "Koguaeg: " + milliseconds + " ms";

                // Võrdleme ainult edukalt lõpetatud töid
                if (completed)
                {
                    if (parallel)
                        parallelTime = milliseconds;
                    else
                        sequentialTime = milliseconds;
                }

                if (sequentialTime.HasValue &&
                    parallelTime.HasValue)
                {
                    Comparison =
                        "Järjestikune: " + sequentialTime.Value + " ms" +
                        " | Paralleelne: " + parallelTime.Value + " ms";

                    if (parallelTime.Value < sequentialTime.Value)
                        Comparison += " | Paralleelne oli kiirem.";
                    else if (parallelTime.Value > sequentialTime.Value)
                        Comparison += " | Järjestikune oli kiirem.";
                    else
                        Comparison += " | Ajad olid võrdsed.";
                }
                else
                {
                    Comparison = "Võrdluseks käivita mõlemad režiimid.";
                }

                IsRunning = false;
                cancellation?.Dispose();
                cancellation = null;
            }
        }

        // Arvutuste tühistamine
        public void Cancel()
        {
            if (IsRunning)
            {
                cancellation?.Cancel();
                Message = "Tühistamine...";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Notify(string name)
        {
            PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(name));
        }
    }

    // Üks robot koos valikuga
    public class RobotChoice
    {
        public Robot Robot { get; }
        public bool IsSelected { get; set; }

        public string Name
        {
            get { return Robot.Name + " (" + Robot.RobotType + ")"; }
        }

        public RobotChoice(Robot robot)
        {
            Robot = robot;
        }
    }
}
