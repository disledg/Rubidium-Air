using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rubidium
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigation;
        private readonly FlightRepo _flightRepo;
        private readonly FlightService _flighService;

        // Команды навигации
        public ICommand NavigateToFlightsCommand { get; }
        public ICommand NavigateToEmployeesCommand { get; }
        public ICommand NavigateToBaggageCommand { get; }

        public MainViewModel(NavigationService navigation, FlightRepo flightRepo, FlightService flightService)
        {
            _navigation = navigation;
            _flightRepo = flightRepo;
            _flighService = flightService;
            // Регистрация страниц
            _navigation.RegisterPage("Flights", () => new FlightsViewModel(_flightRepo, flightService));
            _navigation.RegisterPage("Employees", () => new EmployeesViewModel());
            _navigation.RegisterPage("Baggage", () => new BaggageViewModel());

            // Инициализация команд
            NavigateToFlightsCommand = new RelayCommand(() => NavigateTo("Flights"));
            NavigateToEmployeesCommand = new RelayCommand(() => NavigateTo("Employees"));
            NavigateToBaggageCommand = new RelayCommand(() => NavigateTo("Baggage"));

            // Загрузка начальной страницы
            NavigateTo("Flights");
        }

        public object CurrentPage => _navigation.CurrentPage;

        private void NavigateTo(string pageKey)
        {
            _navigation.NavigateTo(pageKey);
            Debug.WriteLine("Кнопка нажата!");
            OnPropertyChanged(nameof(CurrentPage));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
