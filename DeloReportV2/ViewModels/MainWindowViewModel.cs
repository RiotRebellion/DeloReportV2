using Delo.DAL.Entities;
using Delo.Interfaces;
using Infrastructure.Commands;
using ReportTemplates;
using ReportTemplates.Templates.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ViewModels.Base;


namespace ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {

        #region Properties

        #region Templates

        private ObservableCollection<Template> _templates;

        public ObservableCollection<Template> Templates
        {
            get => _templates;
        }

        #endregion

        #region SelectedTemplate

        private Template selectedTemplate;

        public Template SelectedTemplate 
        { 
            get => selectedTemplate; 
            set => Set(ref selectedTemplate, value); 
        }

        #endregion

        #region PersonRepository

        private IRepository<Person> _personRepository;

        public IRepository<Person> PersonRepository
        {
            get => _personRepository;
            set => Set(ref _personRepository, value);
        }

        #endregion

        #region PersonCollection

        public ObservableCollection<Person> _personCollection => new ObservableCollection<Person>(PersonRepository.Items.ToList());

        public ObservableCollection<Person> PersonCollection
        {
            get => _personCollection;
        }

        #endregion

        #region PersonSourceCollection

        private CollectionViewSource personSourceCollection;

        public ICollectionView PersonSourceCollection
        {
            get => this.personSourceCollection.View;
        }

        #endregion

        #region SelectedPerson

        public Person SelectedPerson { get; set; }

        #endregion

        #region ChoosenPersonCollection

        private ObservableCollection<Person> _choosenPersonCollection = new ObservableCollection<Person>();

        public ObservableCollection<Person> ChoosenPersonCollection
        {
            get => _choosenPersonCollection;
            set => Set(ref _choosenPersonCollection, value);
        }

        #endregion

        #region Status
        private string _status;

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        #endregion

        #region FirstDateTime

        private DateTime _firstDateTime = new DateTime(2021, 1, 1);

        public DateTime FirstDateTime
        {
            get => _firstDateTime;
            set => Set(ref _firstDateTime, value);
        }

        #endregion

        #region SecondDateTime

        private DateTime _LastDateTime = DateTime.Now;

        public DateTime LastDateTime
        {
            get => _LastDateTime;
            set => Set(ref _LastDateTime, value);
        }

        #endregion

        #region FilterText

        private string _filterText;

        public string FilterText
        {
            get => _filterText;
            set 
            {
                Set(ref _filterText, value);
                this.personSourceCollection.View.Refresh();
                
            } 
        }

        #endregion

        #endregion

        #region Commands

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {

        }

        #endregion

        #region CreatePersonList



        #endregion

        #region EditPersonList

        public ICommand EditPersonList { get; }

        private bool CanEditPersonListExecute(object p) => true;

        private void OnEditPersonListExecuted(object p)
        {

        }

        #endregion

        #region AddPerson
        public ICommand AddPersonCommand { get; }

        private bool CanAddPersonCommandExecute(object p) => !PersonCollection.IsNullOrEmpty();

        private void OnAddPersonCommandExecuted(object p)
        {
            if (ChoosenPersonCollection.Contains(SelectedPerson)) return;
            ChoosenPersonCollection.Add(SelectedPerson);
        }

        #endregion

        #region DeletePerson

        public ICommand DeletePersonCommand { get; }

        private bool CanDeletePersonCommandExecute(object p) => !ChoosenPersonCollection.IsNullOrEmpty();

        private void OnDeletePersonCommandExecuted(object p)
        {
            ChoosenPersonCollection.Remove(SelectedPerson);
        }

        #endregion

        #region ClearChoosenPersonListCommand

        public ICommand ClearChoosenPersonListCommand { get; }

        public bool CanClearChoosenPersonListCommandExecute(object p) => !ChoosenPersonCollection.IsNullOrEmpty();

        public void OnClearChoosenPersonListCommandExecuted(object p)
        {
            ChoosenPersonCollection.Clear();
        }

        #endregion

        #region GetReportCommand

        public ICommand GetReportCommand { get; }

        public bool CanGetReportCommandExecute(object p) => (!SelectedTemplate.IsNull() && !ChoosenPersonCollection.IsNullOrEmpty());
                
        

        public void OnGetReportCommandExecuted(object p)
        {
            SelectedTemplate.Outputing(ChoosenPersonCollection, FirstDateTime, LastDateTime);
        }

        #endregion

        #region ClearFilterCommand

        public ICommand ClearFilterCommand { get; }

        public bool CanClearFilterCommandExecute(object p) => !FilterText.IsNullOrEmpty();

        public void OnClearFilterCommandExecuted(object p)
        {
            FilterText = "";
        }

        #endregion

        #endregion

        #region EventHandlers

        #region PersonCollectionView_Filter

        void PersonCollectionView_Filter(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                e.Accepted = true;
                return;
            }

            Person usr = e.Item as Person;
            if (usr.Name.ToUpper().Contains(FilterText.ToUpper()))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        #endregion

        #endregion

        #region Constructor - MainWindowViewModel

        public MainWindowViewModel(IRepository<Person> personRepository)
        {

            _personRepository = personRepository;
            TemplateCollection _templateCollection = new TemplateCollection();
            _templates = _templateCollection.Templates;

            #region SourceCollection

            personSourceCollection = new CollectionViewSource();
            personSourceCollection.Source = PersonCollection;
            personSourceCollection.Filter += PersonCollectionView_Filter;

            #endregion

            #region Commands

            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            EditPersonList = new RelayCommand(OnEditPersonListExecuted, CanEditPersonListExecute);

            AddPersonCommand = new RelayCommand(OnAddPersonCommandExecuted, CanAddPersonCommandExecute);

            DeletePersonCommand = new RelayCommand(OnDeletePersonCommandExecuted, CanDeletePersonCommandExecute);

            ClearChoosenPersonListCommand = new RelayCommand(OnClearChoosenPersonListCommandExecuted, CanClearChoosenPersonListCommandExecute);

            GetReportCommand = new RelayCommand(OnGetReportCommandExecuted, CanGetReportCommandExecute);

            ClearFilterCommand = new RelayCommand(OnClearFilterCommandExecuted, CanClearFilterCommandExecute);

            #endregion

            #region ConnectionCheck

            Status = ConnectionCheck(personRepository);

            #endregion

        }

        #endregion

        #region Functions

        private string ConnectionCheck(IRepository<Person> repository) => repository.CanConnect<Person>()
            ? "Подключено"
            : "Не подключено";

        #endregion

    }
}