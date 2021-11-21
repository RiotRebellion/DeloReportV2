using Delo.DAL.Entities;
using Delo.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Base;


namespace ViewModels
{
    public class PersonListViewModel : ViewModel
    {
        private MainWindowViewModel MainViewModel { get; }

        public PersonListViewModel(MainWindowViewModel MainViewModel, IRepository<Person> PersonRepository)
        {
            MainViewModel = MainViewModel;
        }
    }
}
