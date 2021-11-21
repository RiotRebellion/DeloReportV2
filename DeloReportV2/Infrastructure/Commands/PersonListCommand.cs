using DeloReportV2.Views.Windows;
using Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeloReportV2.Infrastructure.Commands
{
    internal class PersonListCommand : Command
    {
        public PersonListWindow _Window;
        public override bool CanExecute(object parameter) => _Window == null;

        public override void Execute(object parameter)
        {
            var window = new PersonListWindow
            {
                Owner = App.Current.MainWindow
            };
            _Window = window;
            _Window.Closed += OnClosedWindow;
            window.ShowDialog();
        }

        private void OnClosedWindow(object? sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnClosedWindow;
            _Window = null;
        }
    }
}
