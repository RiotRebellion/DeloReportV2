using DeloReportV2;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Base;

namespace ViewModels
{
    public class ViewModelLocator : ViewModel
    {
        public MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public PersonListViewModel PersonListViewModel => App.Services.GetRequiredService<PersonListViewModel>();
    }
}
