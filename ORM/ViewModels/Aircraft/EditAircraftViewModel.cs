using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Rubidium
{
    public class EditAircraftViewModel : INotifyPropertyChanged
    {
        private readonly AircraftService _aircraftService;
        private readonly Window _window;
        private readonly Aircraft _originalAircraft;
        private int _aircraftId;
        private string _registrationNumber;
        private string _model;
        private int _yearOfManufacture;
        private int _passengerCapacity;
        private decimal _cargoCapacity;

        public event PropertyChangedEventHandler PropertyChanged;

        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                _registrationNumber = value;
                OnPropertyChanged(nameof(RegistrationNumber));
                ((RelayCommand)SaveCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
                ((RelayCommand)SaveCommand)?.RaiseCanExecuteChanged();
            }
        }

        public int YearOfManufacture
        {
            get => _yearOfManufacture;
            set
            {
                _yearOfManufacture = value;
                OnPropertyChanged(nameof(YearOfManufacture));
            }
        }

        public int PassengerCapacity
        {
            get => _passengerCapacity;
            set
            {
                _passengerCapacity = value;
                OnPropertyChanged(nameof(PassengerCapacity));
            }
        }

        public decimal CargoCapacity
        {
            get => _cargoCapacity;
            set
            {
                _cargoCapacity = value;
                OnPropertyChanged(nameof(CargoCapacity));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditAircraftViewModel(Aircraft aircraft, AircraftService aircraftService, Window window)
        {
            _aircraftService = aircraftService;
            _window = window;
            _originalAircraft = aircraft;

            _aircraftId = aircraft.Id;
            RegistrationNumber = aircraft.registration_number;
            Model = aircraft.model;
            YearOfManufacture = aircraft.year_of_manufacture;
            PassengerCapacity = aircraft.passenger_capacity;
            CargoCapacity = aircraft.cargo_capacity;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(RegistrationNumber) &&
                   !string.IsNullOrWhiteSpace(Model);
        }

        private void Save(object parameter)
        {
            try
            {
                _aircraftService.UpdateAircraft(
                    _aircraftId,
                    RegistrationNumber,
                    Model,
                    YearOfManufacture,
                    PassengerCapacity,
                    CargoCapacity);

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