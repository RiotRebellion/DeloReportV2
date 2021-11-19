using Models;
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
        public ObservableCollection<Department> Departments { get; }

        public PersonListViewModel()
        {
            var persons = Enumerable.Range(1, 20).Select(i => new Person 
            {
                Name = $"Name {i}",
                Position = $"Position {i}"
            });

            Departments = new ObservableCollection<Department>(Enumerable.Range(1, 5).Select(i => new Department 
            {
                Name = $"Name {i}",
                Persons = new ObservableCollection<Person>(persons)
            }));
        }
    }
}
