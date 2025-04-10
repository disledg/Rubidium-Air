using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rubidium
{
    public class FlightsViewModel : INotifyPropertyChanged
    {
        private readonly FlightRepo _flightRepo;
        private readonly FlightService _flightService;
        private ObservableCollection<Flight> _flights;
        private Flight _selectedFlight;
        public Flight SelectedFlight
        {
            get => _selectedFlight;
            set
            {
                _selectedFlight = value;
                OnPropertyChanged();
                // Можно обновить состояние команды удаления
                ((RelayCommand)DelFlightCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Flight> Flights
        {
            get => _flights;
            set
            {
                _flights = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddFlightCommand { get; }
        public ICommand DelFlightCommand { get; }
        public ICommand UpdFlightCommand { get; }

        public FlightsViewModel(FlightRepo flightRepo, FlightService flightService)
        {
            _flightRepo = flightRepo;
            _flightService = flightService;
            Flights = new ObservableCollection<Flight>();

            LoadFlights();

            AddFlightCommand = new RelayCommand(AddFlight);
            DelFlightCommand = new RelayCommand(
                execute: DeleteSelectedFlight,
                canExecute: () => SelectedFlight != null
            ); 

            UpdFlightCommand = new RelayCommand(UpdFlights);
        }
        private void AddFlight()
        {
            Flights.Add(new Flight
            {
                flight_number = "NEW-001",
                destination = "Париж",
                departure_time = DateTime.Now.AddDays(1),
                arrival_time = DateTime.Now.AddDays(1),
                status = "Новый"
            });
        }
        private void DelFlight()
        {
            //Flights.Remove(
        }
        private void UpdFlights()
        {

        }
        private void LoadFlights()
        {
            var flightsList = _flightRepo.GetAll(); // Получаем List<Flight>
            Flights = new ObservableCollection<Flight>(flightsList);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void DeleteSelectedFlight()
        {
            if (SelectedFlight != null)
            {
                _flightService.DeleteFlight(SelectedFlight.flight_id); // Предполагая, что у вас есть такой метод
                Flights.Remove(SelectedFlight);
                SelectedFlight = null; // Сбрасываем выделение
            }
        }
    }
}
