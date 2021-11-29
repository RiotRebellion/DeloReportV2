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
            Templates.Add(new ResolutionReportAllControlStateDetailed());
            Templates.Add(new ResolutionReportOnControlStateDetailed());
            Templates.Add(new ResolutionReportNoControlStateDetailed());
            Templates.Add(new ResolutionReportAllControlState());
            Templates.Add(new ResolutionReportOnControlState());
            Templates.Add(new ResolutionReportNoControlState());
        }

    } 
}
