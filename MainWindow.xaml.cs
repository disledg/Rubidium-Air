using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rubidium.UI.UC;
using Rubidium.UI.Pages;

namespace Rubidium
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly Dictionary<string, Page> _pages = new();
        public MainWindow()
        {
            InitializeComponent();
            var dbContext = new AirportDataBaseContext();
            App.Navigator.Initialize(MainFrame);
            this.DataContext = new FlightsViewModel(new FlightRepo(dbContext));
            //_pages.Add("Flights", new FlightsDataGrid());
            //_pages.Add("Baggages", new BaggageDataGrid());
            //_pages.Add("Employees", new EmployeeDataGrid());

            // Устанавливаем начальную страницу
            //MainFrame.Content = _pages["Flights"];

            // Начальная навигация
            App.Navigator.NavigateTo(new FlightsDataGrid());
        }

        public void NavigateTo(string pageKey)
        {
            //if (_pages.TryGetValue(pageKey, out var page))
            //{
            //    MainFrame.Content = page;
            //}
        }

    }
}
