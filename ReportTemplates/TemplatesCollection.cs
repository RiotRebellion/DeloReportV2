using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportTemplates
{
    public class TemplateCollection {

        public ObservableCollection<Template> Templates = new ObservableCollection<Template>();

        public ResolutionReport ResolutionReport = new ResolutionReport();

        public TemplateCollection() 
        {
            Templates.Add(ResolutionReport);
        }

    } 
}
