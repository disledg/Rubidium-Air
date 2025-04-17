using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Rubidium
{
    public class BaggageViewModel : INotifyPropertyChanged
    {
        private readonly BaggageService _baggageService;
        private Baggage _selectedBaggage;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Baggage> Baggage { get; private set; }

        public Baggage SelectedBaggage
        {
            get => _selectedBaggage;
            set
            {
                _selectedBaggage = value;
                OnPropertyChanged(nameof(SelectedBaggage));
            }
        }

        public ICommand AddBaggageCommand { get; }
        public ICommand DelBaggageCommand { get; }
        public ICommand UpdBaggageCommand { get; }

        public BaggageViewModel(BaggageService baggageService)
        {
            _baggageService = baggageService ?? throw new ArgumentNullException(nameof(baggageService));

            // Загрузка данных
            LoadBaggage();

            // Инициализация команд
            AddBaggageCommand = new RelayCommand(_ => AddBaggage());
            DelBaggageCommand = new RelayCommand(_ => DeleteBaggage(), _ => SelectedBaggage != null);
            UpdBaggageCommand = new RelayCommand(_ => UpdateBaggage(), _ => SelectedBaggage != null);
        }

        private void LoadBaggage()
        {
            var baggageList = _baggageService.GetAllBaggage().ToList();
            Baggage = new ObservableCollection<Baggage>(baggageList);
            OnPropertyChanged(nameof(Baggage));
        }

        private void AddBaggage()
        {
            var addWindow = new AddBaggageView();
            var addViewModel = new AddBaggageViewModel(this, _baggageService, addWindow);
            addWindow.DataContext = addViewModel;
            addWindow.Owner = Application.Current.MainWindow; // Делаем главное окно владельцем
            addWindow.ShowDialog();
        }

        private void DeleteBaggage()
        {
            if (SelectedBaggage == null) return;

            try
            {
                _baggageService.RemoveBaggage(SelectedBaggage.Id);
                Baggage.Remove(SelectedBaggage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении багажа: {ex.Message}");
            }
        }

        private void UpdateBaggage()
        {
            if (SelectedBaggage != null)
            {
                var editWindow = new EditBaggageView();
                var editViewModel = new EditBaggageViewModel(SelectedBaggage, _baggageService, editWindow);
                editWindow.DataContext = editViewModel;
                editWindow.Owner = Application.Current.MainWindow;
                editWindow.ShowDialog();

                LoadBaggage(); // Обновляем список после редактирования
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
