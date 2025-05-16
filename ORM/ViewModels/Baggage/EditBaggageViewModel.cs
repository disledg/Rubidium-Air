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
    public class EditBaggageViewModel : INotifyPropertyChanged
    {
        private readonly BaggageService _baggageService;
        private readonly Window _window;
        private Baggage _originalBaggage;
        private int _baggageId;
        private int _passengerNumber;
        private string _passengerName;
        private string _passengerSername;
        private int _flightId;
        private decimal _weight;
        private string _status;
        private List<string> _baggageStatuses;

        public event PropertyChangedEventHandler PropertyChanged;

        public EditBaggageViewModel(Baggage baggage,
                                  BaggageService baggageService,
                                  Window window)
        {
            _baggageService = baggageService;
            _window = window;
            _originalBaggage = baggage;

            // Заполняем свойства данными из переданного багажа
            _baggageId = baggage.Id;
            PassengerNumber = baggage.passenger_number;
            PassengerName = baggage.passenger_name;
            PassengerSername = baggage.passenger_sername;
            FlightId = baggage.flight_id;
            Weight = baggage.weight;
            Status = baggage.status;

            // Инициализируем список статусов багажа
            BaggageStatuses = new List<string> { "Registered", "Loading", "In Transit", "Arrived", "Claimed", "Lost" };

            // Инициализация команд
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Свойства для привязки в UI

        public int PassengerNumber
        {
            get { return _passengerNumber; }
            set
            {
                if (_passengerNumber != value)
                {
                    _passengerNumber = value;
                    OnPropertyChanged(nameof(PassengerNumber));
                }
            }
        }

        public string PassengerName
        {
            get { return _passengerName; }
            set
            {
                if (_passengerName != value)
                {
                    _passengerName = value;
                    OnPropertyChanged(nameof(PassengerName));
                }
            }
        }

        public string PassengerSername
        {
            get { return _passengerSername; }
            set
            {
                if (_passengerSername != value)
                {
                    _passengerSername = value;
                    OnPropertyChanged(nameof(PassengerSername));
                }
            }
        }

        public int FlightId
        {
            get { return _flightId; }
            set
            {
                if (_flightId != value)
                {
                    _flightId = value;
                    OnPropertyChanged(nameof(FlightId));
                }
            }
        }

        public decimal Weight
        {
            get { return _weight; }
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    OnPropertyChanged(nameof(Weight));
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

        public List<string> BaggageStatuses
        {
            get { return _baggageStatuses; }
            set
            {
                _baggageStatuses = value;
                OnPropertyChanged(nameof(BaggageStatuses));
            }
        }

        // Команды
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private bool CanSave(object parameter)
        {
            // Валидация данных - проверяем, что выбран статус
            return !string.IsNullOrWhiteSpace(Status);
        }

        private void Save(object parameter)
        {
            try
            {
                // Вызываем метод сервиса для обновления статуса багажа
                _baggageService.UpdateBaggageStatus(
                    _baggageId,
                    Status
                );

                // Закрываем окно после успешного сохранения
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса багажа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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