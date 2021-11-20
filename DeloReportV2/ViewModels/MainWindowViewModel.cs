using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Delo.DAL.Entities;
using Delo.Interfaces;
using Infrastructure;
using Infrastructure.Commands;
using ViewModels.Base;


namespace ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region Repositories

        private readonly IRepository<Person> _PersonRepository;

        #endregion

        #region Status
        private string _Status;

        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
        #endregion

        #region Commands

        #region OpenPersonWindowCommand

        private bool _CanExecute;

        public bool CanExecute
        {
            get => _CanExecute;
            set => Set(ref _CanExecute, value);
        }
        public ICommand OpenPersonWindowCommand { get; }

        private bool CanOpenPersonWindowCommandExecuted(object p) => CanExecute;

        private void OnOpenPersonsWindowCommandExecute(object p)
        {

        }

        #endregion 

        #endregion

        public MainWindowViewModel(IRepository<Person> PersonRepository)
        {
            #region Commands

            OpenPersonWindowCommand = new RelayCommand(OnOpenPersonsWindowCommandExecute, CanOpenPersonWindowCommandExecuted);

            #endregion

            #region ConnectionCheck

            (_CanExecute, Status) = ConnectionCheck(PersonRepository);

            #endregion


        }

        private (bool, string) ConnectionCheck(IRepository<Person> repository) => repository.CanConnect<Person>()
            ? (true, "Подключено")
            : (false, "Не подключено");
    }
}