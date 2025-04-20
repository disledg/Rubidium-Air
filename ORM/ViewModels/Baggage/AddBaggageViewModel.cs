using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rubidium
{
    public class AddBaggageViewModel : INotifyPropertyChanged
    {
        private readonly BaggageService _baggageService;
        private readonly Window _window;
        private readonly BaggageViewModel _parentViewModel;

        private int _passengerNumber;
        private string _passengerName;
        private string _passengerSername;
        private int _flightId;
        private decimal _weight;
        private string _status = "Registered";

        private ObservableCollection<Flight> _availableFlights = new ObservableCollection<Flight>();
        public ObservableCollection<Flight> AvailableFlights
        {
            get => _availableFlights;
            set
            {
                _availableFlights = value;
                OnPropertyChanged(nameof(AvailableFlights));
            }
        }
        public int PassangerNumber
        {
            get => _passengerNumber;
            set
            {
                _passengerNumber = value;
                OnPropertyChanged(nameof(PassangerNumber));
            }
        }

        public string PassengerName
        {
            get => _passengerName;
            set
            {
                _passengerName = value;
                OnPropertyChanged(nameof(PassengerName));
            }
        }

        public string PassengerSername
        {
            get => _passengerSername;
            set
            {
                _passengerSername = value;
                OnPropertyChanged(nameof(PassengerSername));
            }
        }

        public int FlightId
        {
            get => _flightId;
            set
            {
                _flightId = value;
                OnPropertyChanged(nameof(FlightId));
            }
        }

        public decimal Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
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

        public AddBaggageViewModel(BaggageViewModel parentViewModel,
                                 BaggageService baggageService,
                                 Window window)
        {
            _window = window;
            _baggageService = baggageService;
            _parentViewModel = parentViewModel;

            LoadAvailableFlights();

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save(object parameter)
        {
            try
            {
                _baggageService.RegisterBaggage(
                    PassengerSername,
                    Weight,
                    FlightId,
                    PassengerName,
                    PassangerNumber
                );

                _window.DialogResult = true;
                _parentViewModel.LoadBaggage();
                _window.Close();
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadAvailableFlights()
        {
            try
            {
                var flights = _baggageService.GetAllFlights()
                    .OrderBy(f => f.departure_time)  // Сортировка по времени вылета
                    .ToList();

                AvailableFlights.Clear();
                foreach (var flight in flights)
                {
                    AvailableFlights.Add(flight);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке рейсов: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
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
