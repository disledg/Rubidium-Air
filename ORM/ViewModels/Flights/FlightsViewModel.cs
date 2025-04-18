using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Rubidium.ORM.ViewModels.Flights;

namespace Rubidium
{
    public class FlightsViewModel : INotifyPropertyChanged
    {
        private readonly FlightService _flightService;
        private ObservableCollection<Flight> _flights;
        private Flight _selectedFlight;

        public event PropertyChangedEventHandler PropertyChanged;

        public Flight SelectedFlight
        {
            get => _selectedFlight;
            set
            {
                _selectedFlight = value;
                OnPropertyChanged();
                ((RelayCommand)DelFlightCommand)?.RaiseCanExecuteChanged();
                ((RelayCommand)UpdFlightCommand)?.RaiseCanExecuteChanged();
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

        public FlightsViewModel(FlightService flightService)
        {
            try
            {
                _flightService = flightService ?? throw new ArgumentNullException(nameof(flightService));
                Flights = new ObservableCollection<Flight>();

                LoadFlights();

                AddFlightCommand = new RelayCommand(_ => AddFlight());
                DelFlightCommand = new RelayCommand(
                    execute: _ => DeleteSelectedFlight(),
                    canExecute: _ => SelectedFlight != null
                );
                UpdFlightCommand = new RelayCommand(
                    execute: _ => UpdFlights(),
                    canExecute: _ => SelectedFlight != null
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации модели представления рейсов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Flights = new ObservableCollection<Flight>();
            }
        }

        private void AddFlight()
        {
            try
            {
                var addWindow = new AddFlightsView();
                var addViewModel = new AddFlightsViewModel(this, _flightService, addWindow);
                addWindow.DataContext = addViewModel;
                addWindow.Owner = Application.Current.MainWindow;
                addWindow.ShowDialog();

                // Обновляем список после добавления
                LoadFlights();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении рейса: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdFlights()
        {
            if (SelectedFlight == null) return;

            try
            {
                var editWindow = new EditFlightView();
                var editViewModel = new EditFlightsViewModel(SelectedFlight, _flightService, editWindow);
                editWindow.DataContext = editViewModel;
                editWindow.Owner = Application.Current.MainWindow;
                editWindow.ShowDialog();

                LoadFlights();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании рейса: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadFlights()
        {
            try
            {
                var flightsList = _flightService.GetAllFlights();
                Flights = new ObservableCollection<Flight>(flightsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка рейсов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Flights = new ObservableCollection<Flight>();
            }
        }

        private void DeleteSelectedFlight()
        {
            if (SelectedFlight == null) return;

            try
            {
                // Сохраняем выбранный рейс перед удалением
                var flightToDelete = SelectedFlight;
                _flightService.DeleteFlight(flightToDelete.Id);
                Flights.Remove(flightToDelete);
                SelectedFlight = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении рейса: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при обновлении свойства {name}: {ex.Message}");
            }
        }
    }
}