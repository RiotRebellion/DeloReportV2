using Delo.DAL.Entities;
using ReportTemplates.Templates.Base;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System;

namespace ReportTemplates.Templates
{
    public class ResolutionReportOnControlStateDetailed : Template
    {
        public string Name { get; set; } = "Подробный отчёт по поручениям (контролируемые)";

        public override void Outputing(ObservableCollection<Person> personCollection, DateTime firstDate, DateTime lastDate)
        {
            //TODO: Сделать функцию выгрузки в Excel
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
