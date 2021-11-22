using Delo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeloReportV2.Views.Windows
{
    #region DependencyProperties

    #region PersonCollectionProperty

    public static readonly DependencyProperty PersonCollectionProperty =
        DependencyProperty.Register(
        nameof(PersonCollection),
        typeof(ObservableCollection<Person>),
        typeof(Window),
        new PropertyMetadata(default(ObservableCollection<Person>)));

    public ObservableCollection<Person> PersonCollection = new ObservableCollection<Person>();

    #endregion

    #region ChoosenDependencyProperty

    public static readonly DependencyProperty ChoosenPersonsCollectionProperty =
        DependencyProperty.Register(
            nameof(ChoosenPersonsCollection),
            typeof(ObservableCollection<Person>),
            typeof(Window),
            new PropertyMetadata(default(ObservableCollection<Person>)));

    #endregion

    #endregion

    public partial class PersonListWindow : Window
    {
        public PersonListWindow()
        {
            InitializeComponent();
        }
    }
}
