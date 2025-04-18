using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rubidium.ORM.ViewModels.Flights
{
    public class AddFlightsViewModel : INotifyPropertyChanged
    {
        private readonly FlightService _flightService;
        private readonly FlightsViewModel _parentViewModel;
        private readonly Window _window;

        private string _flightNumber;
        private string _destination;
        private DateTime _departureTime = DateTime.Now;
        private DateTime _arrivalTime = DateTime.Now.AddHours(2);
        private string _status = "Scheduled";

        public string FlightNumber
        {
            get => _flightNumber;
            set
            {
                _flightNumber = value;
                OnPropertyChanged(nameof(FlightNumber));
            }
        }

        public string Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                OnPropertyChanged(nameof(Destination));
            }
        }

        public DateTime DepartureTime
        {
            get => _departureTime;
            set
            {
                _departureTime = value;
                OnPropertyChanged(nameof(DepartureTime));
            }
        }

        public DateTime ArrivalTime
        {
            get => _arrivalTime;
            set
            {
                _arrivalTime = value;
                OnPropertyChanged(nameof(ArrivalTime));
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddFlightsViewModel(FlightsViewModel parentViewModel,
                                 FlightService flightService,
                                 Window window)
        {
            _window = window;
            _flightService = flightService;
            _parentViewModel = parentViewModel;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(FlightNumber) &&
                   !string.IsNullOrWhiteSpace(Destination) &&
                   DepartureTime < ArrivalTime;
        }

        private void Save(object parameter)
        {
            try
            {
                _flightService.CreateFlight(
                    FlightNumber,
                    Destination,
                    DepartureTime,
                    ArrivalTime,
                    Status
                );

                _window.DialogResult = true;
                _parentViewModel.LoadFlights();
                _window.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object parameter)
        {
            _window.DialogResult = false;
            _window.Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
