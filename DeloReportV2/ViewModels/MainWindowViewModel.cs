using Delo.DAL.Entities;
using Delo.Interfaces;
using Infrastructure.Commands;
using Infrastructure.Commands.Base;
using System.Windows.Input;
using ViewModels.Base;


namespace ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region PersonRepository

        private IRepository<Person> _PersonRepository;

        public IRepository<Person> PersonRepository
        {
            get => _PersonRepository;
            set => Set(ref _PersonRepository, value);
        }

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

        #region CloseApplicationCommand

        public ICommand CloseApplication { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            
        }

        #endregion

        #region CreatePersonList

        public ICommand CreatePersonList { get; }

        private bool CanCreatePersonListExecute(object p) => true;
        
        private void OnCreatePersonListExecuted(object p)
        {
            var Persons = (IRepository<Person>)p;

            var dialog = new PersonListViewModel
            {

            };
        }

        #endregion

        #region EditPersonList

        public ICommand EditPersonList { get; }

        private bool CanEditPersonListExecute(object p) => true;

        private void OnEditPersonListExecuted(object p)
        {

        }

        #endregion

        #endregion

        public MainWindowViewModel(IRepository<Person> PersonRepository)
        {
            #region Commands

            CloseApplication = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            CreatePersonList = new RelayCommand(OnCreatePersonListExecuted, CanCreatePersonListExecute);
            EditPersonList = new RelayCommand(OnEditPersonListExecuted, CanEditPersonListExecute);

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