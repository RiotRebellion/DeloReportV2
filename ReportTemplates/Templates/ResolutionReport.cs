using Delo.DAL.Entities;
using ReportTemplates.Templates.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportTemplates.Templates
{
    public class ResolutionReport : Template
    {   
        public string Name { get; set; } = "Отчёт по поручениям";

        public void Outputing(ObservableCollection<Person> personCollection)
        {
            
        }
    }
}
