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
        #region Repository
        private readonly IRepository<Person> _PersonRepository;

        #endregion

        #region Title
        private string _Title = "Отчёты СЭД ДЕЛО";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region Status
        private readonly string _Status = "Нет подключения";
        public string Status
        {
            get => _Status;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Commands

        #region OpenPersonWindowCommand

        public ICommand OpenPersonWindowCommand { get; }

        private bool CanOpenPersonWindowCommandExecuted(object p) => true;

        private void OnOpenPersonsWindowCommandExecute(object p)
        {

        }

        #endregion 

        #endregion

        public MainWindowViewModel()
        {
            #region Commands

            OpenPersonWindowCommand = new RelayCommand(OnOpenPersonsWindowCommandExecute, CanOpenPersonWindowCommandExecuted);


            #endregion

            #region PersonRepository

            //_PersonRepository = PersonRepository;


            #endregion
        }
    }
}
