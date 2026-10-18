
using System;
using System.Windows.Input;

namespace RobotPlanet
{
    public class RelayCommand : ICommand
    {
        private readonly Action action;
        private readonly Func<bool>? canExecute;

        public RelayCommand(Action action, Func<bool>? canExecute = null)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object? parameter)
        {
            action();
        }

        public event EventHandler? CanExecuteChanged;

        public void Update()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
