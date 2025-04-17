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
using Rubidium.ORM.ViewModels.Flights;

namespace Rubidium
{
    public class FlightsViewModel : INotifyPropertyChanged
    {
        private readonly FlightService _flightService;
        private ObservableCollection<Flight> _flights;
        private Flight _selectedFlight;
        public Flight SelectedFlight
        {
            get => _selectedFlight;
            set
            {
                _selectedFlight = value;
                OnPropertyChanged();
                // Можно обновить состояние команды удаления
                ((RelayCommand)DelFlightCommand).RaiseCanExecuteChanged();
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
                var editViewModel = new EditFlightsViewModel(SelectedFlight, _flightService, editWindow);
                editWindow.DataContext = editViewModel;
                editWindow.Owner = Application.Current.MainWindow;
                editWindow.ShowDialog();

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
