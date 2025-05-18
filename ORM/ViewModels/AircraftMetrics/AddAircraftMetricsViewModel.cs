using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Rubidium
{
    public class AddAircraftMetricsViewModel : INotifyPropertyChanged
    {
        private readonly AircraftMetricsService _metricsService;
        private readonly Window _window;
        private readonly int _aircraftId;
        private decimal _fuelConsumption;
        private int _passengerLoad;
        private decimal _cargoLoad;
        private decimal _waterLevel;
        private decimal _oilLevel;
        private decimal _technicalCondition;

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal FuelConsumption
        {
            get => _fuelConsumption;
            set
            {
                _fuelConsumption = value;
                OnPropertyChanged(nameof(FuelConsumption));
            }
        }

        public int PassengerLoad
        {
            get => _passengerLoad;
            set
            {
                _passengerLoad = value;
                OnPropertyChanged(nameof(PassengerLoad));
            }
        }

        public decimal CargoLoad
        {
            get => _cargoLoad;
            set
            {
                _cargoLoad = value;
                OnPropertyChanged(nameof(CargoLoad));
            }
        }

        public decimal WaterLevel
        {
            get => _waterLevel;
            set
            {
                _waterLevel = value;
                OnPropertyChanged(nameof(WaterLevel));
            }
        }

        public decimal OilLevel
        {
            get => _oilLevel;
            set
            {
                _oilLevel = value;
                OnPropertyChanged(nameof(OilLevel));
            }
        }

        public decimal TechnicalCondition
        {
            get => _technicalCondition;
            set
            {
                _technicalCondition = value;
                OnPropertyChanged(nameof(TechnicalCondition));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddAircraftMetricsViewModel(AircraftMetricsService metricsService, int aircraftId, Window window)
        {
            _metricsService = metricsService;
            _window = window;
            _aircraftId = aircraftId;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            // Установка значений по умолчанию
            TechnicalCondition = 100;
            WaterLevel = 100;
            OilLevel = 100;
        }

        private void Save(object parameter)
        {
            try
            {
                _metricsService.AddMetrics(
                    _aircraftId,
                    FuelConsumption,
                    PassengerLoad,
                    CargoLoad,
                    WaterLevel,
                    OilLevel,
                    TechnicalCondition);

                _window.DialogResult = true;
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object parameter)
        {
            _window.DialogResult = false;
            _window.Close();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 