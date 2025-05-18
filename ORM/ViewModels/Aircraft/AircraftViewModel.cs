using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Rubidium
{
    public class AircraftViewModel : INotifyPropertyChanged
    {
        private readonly AircraftService _aircraftService;
        private ObservableCollection<Aircraft> _aircraft;
        private Aircraft _selectedAircraft;

        public event PropertyChangedEventHandler PropertyChanged;

        public Aircraft SelectedAircraft
        {
            get => _selectedAircraft;
            set
            {
                _selectedAircraft = value;
                OnPropertyChanged(nameof(SelectedAircraft));
                ((RelayCommand)DelAircraftCommand)?.RaiseCanExecuteChanged();
                ((RelayCommand)UpdAircraftCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Aircraft> Aircraft
        {
            get => _aircraft;
            set
            {
                _aircraft = value;
                OnPropertyChanged(nameof(Aircraft));
            }
        }

        public ICommand AddAircraftCommand { get; }
        public ICommand DelAircraftCommand { get; }
        public ICommand UpdAircraftCommand { get; }

        public AircraftViewModel(AircraftService aircraftService)
        {
            _aircraftService = aircraftService ?? throw new ArgumentNullException(nameof(aircraftService));
            Aircraft = new ObservableCollection<Aircraft>();

            AddAircraftCommand = new RelayCommand(AddAircraft);
            DelAircraftCommand = new RelayCommand(DeleteAircraft, param => SelectedAircraft != null);
            UpdAircraftCommand = new RelayCommand(UpdateAircraft, param => SelectedAircraft != null);

            LoadAircraft();
        }

        private void LoadAircraft()
        {
            Aircraft.Clear();
            foreach (var aircraft in _aircraftService.GetAllAircraft())
            {
                Aircraft.Add(aircraft);
            }
        }

        private void AddAircraft(object parameter)
        {
            var window = new AddAircraftView();
            var viewModel = new AddAircraftViewModel(_aircraftService, window);
            window.DataContext = viewModel;
            window.ShowDialog();

            LoadAircraft();
        }

        private void DeleteAircraft(object parameter)
        {
            if (SelectedAircraft != null)
            {
                try
                {
                    _aircraftService.DeleteAircraft(SelectedAircraft.Id);
                    LoadAircraft();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private void UpdateAircraft(object parameter)
        {
            if (SelectedAircraft != null)
            {
                var window = new EditAircraftView();
                var viewModel = new EditAircraftViewModel(SelectedAircraft, _aircraftService, window);
                window.DataContext = viewModel;
                window.ShowDialog();

                LoadAircraft();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 