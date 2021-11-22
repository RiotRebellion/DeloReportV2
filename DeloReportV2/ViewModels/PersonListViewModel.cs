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
using System.Windows;
using System.Windows.Input;
using ViewModels.Base;

namespace ViewModels
{
    public class PersonListViewModel : ViewModel
    {
        private readonly IRepository<Person> _PersonRepository;

        #region DependencyProperties

        #region PersonCollectionProperty

        public static readonly DependencyProperty PersonCollectionProperty =
            DependencyProperty.Register(
            nameof(PersonCollection),
            typeof(ObservableCollection<Person>),
            typeof(Window),
            new PropertyMetadata(default(ObservableCollection<Person>)));

        #endregion

        #region ChoosenDependencyProperty

        public static readonly DependencyProperty ChoosenPersonsCollectionProperty =
            DependencyProperty.Register(
                nameof(ChoosenPersonsCollection),
                typeof(ObservableCollection<Person>),
                typeof(Window),
                new PropertyMetadata(default(ObservableCollection<Person>)));

        #endregion

        #endregion

        #region Properties

        #region PersonCollection

        public ObservableCollection<Person> PersonCollection => new ObservableCollection<Person>();

        #endregion

        #region ChoosenPersons

        private ObservableCollection<Person> _ChoosenPersonsCollection = new ObservableCollection<Person>();

        public ObservableCollection<Person> ChoosenPersonsCollection
        {
            get => _ChoosenPersonsCollection;
            set => Set(ref _ChoosenPersonsCollection, value);
        }

        #endregion

        #region SelectedPersons

        public Person SelectedPerson { get; set; }

        #endregion

        #region ChoosenSelectedPerson

        private Person _SelectedChoosenPerson;

        public Person SelectedChoosenPerson
        {
            get => _SelectedChoosenPerson;
            set => Set(ref _SelectedChoosenPerson, value);
        }

        #endregion

        #endregion

        #region Commands

        #region AddPerson
        public ICommand AddPersonCommand { get; }

        private bool CanAddPersonCommandExecute(object p) => !PersonCollection.IsNullOrEmpty();

        private void OnAddPersonCommandExecuted(object p)
        {
            if (ChoosenPersons.Contains(SelectedPerson)) return;
            ChoosenPersons.Add(SelectedPerson);
        }

        #endregion

        #region DeletePerson

        public ICommand DeletePersonCommand { get; }

        private bool CanDeletePersonCommandExecute(object p) => !ChoosenPersons.IsNullOrEmpty();

        private void OnDeletePersonCommandExecuted(object p)
        {
            ChoosenPersons.Remove(SelectedChoosenPerson);
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
