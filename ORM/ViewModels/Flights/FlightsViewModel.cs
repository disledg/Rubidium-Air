using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Rubidium.ORM.ViewModels.Flights;
=======
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Rubidium;
>>>>>>> origin/Develop

namespace Rubidium
{
    public class FlightsViewModel : INotifyPropertyChanged
    {
        private readonly FlightService _flightService;
        private ObservableCollection<Flight> _flights;
        private Flight _selectedFlight;
<<<<<<< HEAD
=======

        public event PropertyChangedEventHandler PropertyChanged;

>>>>>>> origin/Develop
        public Flight SelectedFlight
        {
            get => _selectedFlight;
            set
            {
                _selectedFlight = value;
                OnPropertyChanged();
<<<<<<< HEAD
                // Можно обновить состояние команды удаления
                ((RelayCommand)DelFlightCommand).RaiseCanExecuteChanged();
=======
                ((RelayCommand)DelFlightCommand)?.RaiseCanExecuteChanged();
                ((RelayCommand)UpdFlightCommand)?.RaiseCanExecuteChanged();
>>>>>>> origin/Develop
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
<<<<<<< HEAD
=======

>>>>>>> origin/Develop
        public ICommand AddFlightCommand { get; }
        public ICommand DelFlightCommand { get; }
        public ICommand UpdFlightCommand { get; }

        public FlightsViewModel(FlightService flightService)
        {
<<<<<<< HEAD
            _flightService = flightService;
            Flights = new ObservableCollection<Flight>();

            LoadFlights();

            AddFlightCommand = new RelayCommand(_ => AddFlight());
            DelFlightCommand = new RelayCommand(
                execute: _ => DeleteSelectedFlight(),
                canExecute: _ => SelectedFlight != null
            ); 

            UpdFlightCommand = new RelayCommand(_ => UpdFlights());
        }
        private void AddFlight()
        {
            var addWindow = new AddFlightsView();
            var addViewModel = new AddFlightsViewModel(this, _flightService, addWindow);
            addWindow.DataContext = addViewModel;
            addWindow.Owner = Application.Current.MainWindow; // Делаем главное окно владельцем
            addWindow.ShowDialog();
        }
        private void UpdFlights()
        {
            if (SelectedFlight != null)
            {
                var editWindow = new EditFlightView();
=======
            try
            {
                _flightService = flightService ?? throw new ArgumentNullException(nameof(flightService));
                Flights = new ObservableCollection<Flight>();
                UpdFlightCommand = new RelayCommand(UpdateFlight, param => SelectedFlight != null);

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
                var editWindow = new EditFlightView(SelectedFlight, _flightService);
>>>>>>> origin/Develop
                var editViewModel = new EditFlightsViewModel(SelectedFlight, _flightService, editWindow);
                editWindow.DataContext = editViewModel;
                editWindow.Owner = Application.Current.MainWindow;
                editWindow.ShowDialog();

<<<<<<< HEAD
                LoadFlights(); // Обновляем список после редактирования
            }
        }
        private void LoadFlights()
        {
            var flightsList = _flightService.GetAllFlights(); // Получаем List<Flight>
            Flights = new ObservableCollection<Flight>(flightsList);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void DeleteSelectedFlight()
        {
            if (SelectedFlight != null)
            {
                _flightService.DeleteFlight(SelectedFlight.Id); // Предполагая, что у вас есть такой метод
                Flights.Remove(SelectedFlight);
                SelectedFlight = null; // Сбрасываем выделение
            }
        }
    }
}
=======
                LoadFlights();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании рейса: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateFlight(object parameter)
        {
            if (SelectedFlight != null)
            {
                var editWindow = new EditFlightView(SelectedFlight, _flightService);
                editWindow.ShowDialog();

                RefreshFlights();
            }
            else
            {
                MessageBox.Show("Выберите рейс для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void RefreshFlights()
        {
            var flights = _flightService.GetAllFlights();
            Flights.Clear();
            foreach (var flight in flights)
            {
                Flights.Add(flight);
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
>>>>>>> origin/Develop
