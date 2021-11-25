using Delo.DAL.Entities;
using System.Collections.ObjectModel;

namespace ReportTemplates.Templates.Base
{
    public abstract class Template
    {   
        public string Name { get; set;}

        public abstract void Outputing(ObservableCollection<Person> personCollection, DateTime firstDate, DateTime lastDate);

        public override string ToString()
        {
            return $"{Name}";
        }

    }
}