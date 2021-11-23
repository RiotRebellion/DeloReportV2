using ReportTemplates.Templates.Base;
using ReportTemplates.Templates;
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

        public TemplateCollection() 
        {
            Templates.Add(new ResolutionReport());
        }

    } 
}
