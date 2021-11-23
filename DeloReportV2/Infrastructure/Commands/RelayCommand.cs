using Infrastructure.Commands.Base;
using System;

namespace Infrastructure.Commands
{
    internal class RelayCommand : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool>? _CanExectute;

        public RelayCommand(Action<object> Execute, Func<object, bool>? CanExectute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExectute = CanExectute;
        }

        public override bool CanExecute(object parameter) => _CanExectute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;
            _Execute(parameter);
        }
    }
}
