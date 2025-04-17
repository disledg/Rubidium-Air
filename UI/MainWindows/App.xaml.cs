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
            var dbContext = new AirportDBEntities1();
            Navigator = new NavigationService();

            var flightService = new FlightService(new FlightRepo(dbContext), new BaggageRepo(dbContext));
            var employeeService = new EmployeeService(new EmployeeRepo(dbContext), new FlightRepo(dbContext));
            var baggageService = new BaggageService(new BaggageRepo(dbContext),new FlightRepo(dbContext));
            var authService = new AuthService(new UserRepository(dbContext, new PasswordHasher()),new  PasswordHasher());
            // Регистрируем страницы
            Navigator.RegisterPage("Flights", () => new FlightsViewModel(flightService));
            Navigator.RegisterPage("Employees", () => new EmployeesViewModel(employeeService));
            Navigator.RegisterPage("Baggage", () => new BaggageViewModel(baggageService));

            var AuthVM = new AuthViewModel(authService, employeeService, flightService, baggageService, Navigator);

            var login = new login
            {
                DataContext = AuthVM
            };

            login.Show();
        }
    }
}
