using Delo.DAL.Entities;
using System.Collections.ObjectModel;

namespace ReportTemplates
{
    public interface Template
    {   
        public string Name { get; set;}

        public void Outputing(ObservableCollection<Person> personCollection);
    }
}