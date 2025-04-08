using System;
using System.Collections.Generic;
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
        private List<Flight> _flights;

        public List<Flight> Flights
        {
            get => _flights;
            set
            {
                _flights = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddFlightCommand { get; }
        public FlightsViewModel(FlightRepo flightRepo)
        {
            _flightRepo = flightRepo;
            LoadFlights();
            AddFlightCommand = new RelayCommand(AddFlight);
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
        private void LoadFlights()
        {
            Flights = _flightRepo.GetAll();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
