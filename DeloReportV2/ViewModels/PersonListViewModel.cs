using Delo.DAL.Entities;
using Delo.Interfaces;
using Infrastructure.Commands;
using Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Base;

namespace ViewModels
{
    public class PersonListViewModel : ViewModel
    {
        private readonly IRepository<Person> _PersonRepository;

        #region ChoosenPersons

        private ObservableCollection<Object> _ChoosenPersons = new ObservableCollection<Object>();

        public ObservableCollection<Object> ChoosenPersons
        {
            get => _ChoosenPersons;
            set => Set(ref _ChoosenPersons, value);
        }

        #endregion

        #region SelectedPersons

        private ObservableCollection<Object> _SelectedPersons = new ObservableCollection<object>();

        public ObservableCollection<Object> SelectedPersons
        {
            get => _SelectedPersons;
            set => Set(ref _SelectedPersons, value);
        }

        #endregion



        public IEnumerable<Person> Persons => _PersonRepository.Items;

        #region Commands

        #region AddPerson
        public ICommand AddPersonCommand;

        private bool CanAddPersonCommandExecute(object p) => SelectedPersons.IsNullOrEmpty();

        private void OnAddPersonCommandExecuted(object p)
        {
            ChoosenPersons.AddItems(SelectedPersons);
        }

        #endregion

        #region DeletePerson

        public ICommand DeletePersonCommand;

        private bool CanDeletePersonCommandExecute(object p) => ChoosenPersons.IsNullOrEmpty();

        private void OnDeletePersonCommandExecuted(object p)
        {
            SelectedPersons.RemoveItems(ChoosenPersons);
        }

        #endregion

        #endregion
        public PersonListViewModel(IRepository<Person> PersonRepository)
        {
            _PersonRepository = PersonRepository;

            #region Commands

            AddPersonCommand = new RelayCommand(OnAddPersonCommandExecuted, CanAddPersonCommandExecute);
            DeletePersonCommand = new RelayCommand(OnDeletePersonCommandExecuted, CanDeletePersonCommandExecute);

            #endregion

        }
    }
}
