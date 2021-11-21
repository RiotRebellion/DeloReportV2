using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Delo.DAL.Entities;
using Delo.Interfaces;
using DeloReportV2;
using DeloReportV2.Views.Windows;
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

        #region RemovePerson

        public ICommand DeletePersonCommand;

        private bool CanDeletePersonCommandExecute(object p) => true;

        private void OnDeletePersonCommandExecuted(object p)
        {

        }

        #endregion

        #endregion

        public MainWindowViewModel(IRepository<Person> PersonRepository)
        {
            #region Commands

            DeletePersonCommand = new RelayCommand(OnDeletePersonCommandExecuted, CanDeletePersonCommandExecute);

            #endregion

            #region ConnectionCheck

            Status = ConnectionCheck(PersonRepository);

            #endregion

        }

        private string ConnectionCheck(IRepository<Person> repository) => repository.CanConnect<Person>()
            ? "Подключено"
            : "Не подключено";
    }
}