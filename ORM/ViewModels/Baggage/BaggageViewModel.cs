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
<<<<<<< HEAD
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
=======
            try
            {
                _baggageService = baggageService ?? throw new ArgumentNullException(nameof(baggageService));

                LoadBaggage();

                AddBaggageCommand = new RelayCommand(_ => AddBaggage());
                DelBaggageCommand = new RelayCommand(_ => DeleteBaggage(), _ => SelectedBaggage != null);
                UpdBaggageCommand = new RelayCommand(_ => UpdateBaggage(), _ => SelectedBaggage != null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации ViewModel: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadBaggage()
        {
            try
            {
                var baggageList = _baggageService.GetAllBaggage().ToList();
                Baggage = new ObservableCollection<Baggage>(baggageList);
                OnPropertyChanged(nameof(Baggage));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке багажа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Baggage = new ObservableCollection<Baggage>();
                OnPropertyChanged(nameof(Baggage));
            }
>>>>>>> origin/Develop
        }

        private void AddBaggage()
        {
<<<<<<< HEAD
            var addWindow = new AddBaggageView();
            var addViewModel = new AddBaggageViewModel(this, _baggageService, addWindow);
            addWindow.DataContext = addViewModel;
            addWindow.Owner = Application.Current.MainWindow; // Делаем главное окно владельцем
            addWindow.ShowDialog();
=======
            try
            {
                var addWindow = new AddBaggageView();
                var addViewModel = new AddBaggageViewModel(this, _baggageService, addWindow);
                addWindow.DataContext = addViewModel;
                addWindow.Owner = Application.Current.MainWindow;
                addWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна добавления багажа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
>>>>>>> origin/Develop
        }

        private void DeleteBaggage()
        {
            if (SelectedBaggage == null) return;
<<<<<<< HEAD

=======
>>>>>>> origin/Develop
            try
            {
                _baggageService.RemoveBaggage(SelectedBaggage.Id);
                Baggage.Remove(SelectedBaggage);
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                MessageBox.Show($"Ошибка при удалении багажа: {ex.Message}");
=======
                MessageBox.Show($"Ошибка при удалении багажа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
>>>>>>> origin/Develop
            }
        }

        private void UpdateBaggage()
        {
<<<<<<< HEAD
            if (SelectedBaggage != null)
=======
            if (SelectedBaggage == null) return;

            try
>>>>>>> origin/Develop
            {
                var editWindow = new EditBaggageView();
                var editViewModel = new EditBaggageViewModel(SelectedBaggage, _baggageService, editWindow);
                editWindow.DataContext = editViewModel;
                editWindow.Owner = Application.Current.MainWindow;
                editWindow.ShowDialog();

<<<<<<< HEAD
                LoadBaggage(); // Обновляем список после редактирования
=======
                LoadBaggage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении багажа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
>>>>>>> origin/Develop
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
<<<<<<< HEAD
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
=======
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при обновлении свойства {propertyName}: {ex.Message}");
            }
        }
    }
}
>>>>>>> origin/Develop
