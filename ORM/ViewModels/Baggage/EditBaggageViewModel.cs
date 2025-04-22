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
    internal class EditBaggageViewModel : INotifyPropertyChanged
    {
        private readonly BaggageService _baggageService;
        private readonly Window _window;
        private Baggage _originalBaggage;
        private int _baggageId;
        private int _passenger_number;
        private string _passenger_name;
        private string _passenger_sername;
        private int _flightId;
        private decimal _weight;
        private string _status;

        public event PropertyChangedEventHandler PropertyChanged;

        public EditBaggageViewModel(Baggage baggage,
                                  BaggageService baggageService,
                                  Window window)
        {
            _baggageService = baggageService;
            _window = window;
            _originalBaggage = baggage;

            // Заполняем свойства данными из переданного рейса
            _baggageId = baggage.Id;
            _passenger_number = baggage.passenger_number;
            _passenger_name = baggage.passenger_name;
            _passenger_sername = baggage.passenger_sername;
            _flightId = baggage.flight_id;
            _weight = baggage.weight;
            _status = baggage.status;

            // Инициализация команд
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Свойства для привязки в UI

        public int PassengerNumber
        {
            get { return _passenger_number; }
            set
            {
                if (_passenger_number != value)
                {
                    _passenger_number = value;
                    OnPropertyChanged(nameof(PassengerNumber));
                }
            }
        }

        public string PassengerName
        {
            get { return _passenger_name; }
            set
            {
                if (_passenger_name != value)
                {
                    _passenger_name = value;
                    OnPropertyChanged(nameof(PassengerName));
                }
            }
        }

        public string PassengerSername
        {
            get { return _passenger_sername; }
            set
            {
                if (_passenger_sername != value)
                {
                    _passenger_sername = value;
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
        // Команды
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private bool CanSave(object parameter)
        {
            // Валидация данных
            return !string.IsNullOrWhiteSpace(PassengerName)
                && !string.IsNullOrWhiteSpace(PassengerSername)
                && !string.IsNullOrWhiteSpace(Status)
                && PassengerNumber != 0
                && FlightId != 0 && Weight != 0;
        }

        private void Save(object parameter)
        {
            try
            {
                // Вызываем метод сервиса для обновления рейса с правильными параметрами
                _baggageService.UpdateBaggageStatus(
                    _baggageId,
                    _status
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
