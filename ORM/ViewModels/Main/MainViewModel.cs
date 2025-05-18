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
        private readonly FlightService _flightService;
        private readonly BaggageService _baggageService;
        private readonly EmployeeService _employeeService;
        private readonly AircraftService _aircraftService;
        private readonly AircraftMetricsService _aircraftMetricsService;

        public ICommand NavigateToFlightsCommand { get; }
        public ICommand NavigateToEmployeesCommand { get; }
        public ICommand NavigateToBaggageCommand { get; }
        public ICommand NavigateToAircraftCommand { get; }
        public ICommand NavigateToAircraftMetricsCommand { get; }

        public MainViewModel(NavigationService navigation, FlightService flightService, EmployeeService employeeService, BaggageService baggageService, AircraftService aircraftService, AircraftMetricsService aircraftMetricsService)
        {
            _navigation = navigation;
            _flightService = flightService;
            _employeeService = employeeService;
            _baggageService = baggageService;
            _aircraftService = aircraftService;
            _aircraftMetricsService = aircraftMetricsService;
            _navigation.RegisterPage("Flights", () => new FlightsViewModel(flightService));
            _navigation.RegisterPage("Employees", () => new EmployeesViewModel(employeeService));
            _navigation.RegisterPage("Baggage", () => new BaggageViewModel(baggageService));
            _navigation.RegisterPage("Aircraft", () => new AircraftViewModel(aircraftService));
            _navigation.RegisterPage("AircraftMetrics", () => new AircraftMetricsViewModel(aircraftMetricsService, aircraftService));


            NavigateToFlightsCommand = new RelayCommand(_ => NavigateTo("Flights"));
            NavigateToEmployeesCommand = new RelayCommand(_ => NavigateTo("Employees"));
            NavigateToBaggageCommand = new RelayCommand(_ => NavigateTo("Baggage"));
            NavigateToAircraftCommand = new RelayCommand(_ => NavigateTo("Aircraft"));
            NavigateToAircraftMetricsCommand = new RelayCommand(_ => NavigateTo("AircraftMetrics"));

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
