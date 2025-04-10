using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
namespace Rubidium
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static NavigationService Navigator { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаем и сохраняем NavigationService как статическое свойство
            var dbContext = new AirportDataBaseContext();
            Navigator = new NavigationService();
            var flightRepo = new FlightRepo(dbContext);
            var baggageRepo = new BaggageRepo(dbContext);
            var flightService = new FlightService(flightRepo,baggageRepo);

            // Регистрируем страницы
            Navigator.RegisterPage("Flights", () => new FlightsViewModel(flightRepo, flightService));
            Navigator.RegisterPage("Employees", () => new EmployeesViewModel());
            Navigator.RegisterPage("Baggage", () => new BaggageViewModel());

            var mainVM = new MainViewModel(Navigator, flightRepo, flightService);

            var mainWindow = new MainWindow
            {
                DataContext = mainVM
            };

            mainWindow.Show();
        }
    }
}
