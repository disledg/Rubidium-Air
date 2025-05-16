
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rubidium
{
    internal class EditFlightsViewModel : INotifyPropertyChanged
    {
        private readonly FlightService _flightService;
        private readonly Window _window;
        private Flight _originalFlight;
        private readonly FlightRepo _flightRepo;
        private int _flightId;
        private string _flightNumber;
        private string _destination;
        private DateTime? _departureTime;
        private DateTime? _arrivalTime;
        private string _status;

        public event PropertyChangedEventHandler PropertyChanged;

        public EditFlightsViewModel(Flight flight,
                                  FlightService flightService,
                                  Window window)
        {
            _flightService = flightService;
            _window = window;
            _originalFlight = flight;

            // Заполняем свойства данными из переданного рейса
            _flightId = flight.Id;
            FlightNumber = flight.flight_number;
            Destination = flight.destination;
            DepartureTime = flight.departure_time;
            ArrivalTime = flight.arrival_time;
            Status = flight.status;

            // Инициализация команд
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Свойства для привязки в UI
        
        public string FlightNumber
        {
            get { return _flightNumber; }
            set
            {
                if (_flightNumber != value)
                {
                    _flightNumber = value;
                    OnPropertyChanged(nameof(FlightNumber));
                }
            }
        }

        public string Destination
        {
            get { return _destination; }
            set
            {
                if (_destination != value)
                {
                    _destination = value;
                    OnPropertyChanged(nameof(Destination));
                }
            }
        }

        public DateTime? DepartureTime
        {
            get { return _departureTime; }
            set
            {
                if (_departureTime != value)
                {
                    _departureTime = value;
                    OnPropertyChanged(nameof(DepartureTime));
                }
            }
        }

        public DateTime? ArrivalTime
        {
            get { return _arrivalTime; }
            set
            {
                if (_arrivalTime != value)
                {
                    _arrivalTime = value;
                    OnPropertyChanged(nameof(ArrivalTime));
                }
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        // Команды
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private bool CanSave(object parameter)
        {
            // Валидация данных
            return !string.IsNullOrWhiteSpace(FlightNumber)
                && !string.IsNullOrWhiteSpace(Destination)
                && DepartureTime.HasValue
                && ArrivalTime.HasValue
                && !string.IsNullOrWhiteSpace(Status);
        }

        private void Save(object parameter)
        {
            try
            {
                // Проверяем, что все необходимые данные присутствуют
                if (DepartureTime >= ArrivalTime)
                {
                    MessageBox.Show("Время вылета должно быть раньше прибытия", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Вызываем метод сервиса для обновления рейса с правильными параметрами
                _flightService.UpdateFlight(
                    _flightId,
                    FlightNumber,
                    Destination,
                    DepartureTime.Value,
                    ArrivalTime.Value,
                    Status
                );

                // Закрываем окно после успешного сохранения
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении рейса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object parameter)
        {
            // Просто закрываем окно
            _window.Close();
        }

        // Метод для уведомления об изменении свойств
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}