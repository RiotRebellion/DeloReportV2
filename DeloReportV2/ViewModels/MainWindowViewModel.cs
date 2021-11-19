using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Infrastructure;
using Infrastructure.Commands;
using Models;
using ViewModels.Base;


namespace ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region HeadConnection
        //private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";
        //private static readonly (SqlConnection, string) connectionResult = Connection.Connect(connectionString);
        private readonly SqlConnection? sqlConnection = null;
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

        #region PersonCollection
        private ObservableCollection<Person> _PersonCollection;

        public ObservableCollection<Person> PersonCollection
        {
            get => _PersonCollection;
            set => Set(ref _PersonCollection, value);
        }
        #endregion

        #region Commands

        #region OpenPersonWindowCommand

        public ICommand OpenPersonWindowCommand { get; }

        private bool CanOpenPersonWindowCommandExecuted(object p) =>  sqlConnection != null;

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
        }
    }
}
