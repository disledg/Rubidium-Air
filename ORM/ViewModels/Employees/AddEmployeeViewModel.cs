using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Rubidium
{
    public class AddEmployeeViewModel : INotifyPropertyChanged
    {
        private readonly EmployeesViewModel _parentViewModel;
        private readonly EmployeeService _employeeService;
        private readonly Window _window;

        public AddEmployeeViewModel(EmployeesViewModel parentViewModel,
                                 EmployeeService employeeService,
                                 Window window)
        {
            _parentViewModel = parentViewModel;
            _employeeService = employeeService;
            _window = window;

            SaveCommand = new RelayCommand(SaveEmployee);
            CancelCommand = new RelayCommand(Cancel);
        }

<<<<<<< HEAD
        // Свойства для привязки (аналогично предыдущему примеру)
=======
>>>>>>> origin/Develop
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Position { get; set; }
        public string ContactInfo { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void SaveEmployee(object parameter)
        {
<<<<<<< HEAD
            // Используем сервис вместо прямого доступа к репозиторию
            _employeeService.AddEmployee(
=======
            try
            {
                _employeeService.AddEmployee(
>>>>>>> origin/Develop
                FirstName,
                LastName,
                Position,
                ContactInfo);

<<<<<<< HEAD
            _parentViewModel.LoadEmployees(); // Обновляем список
            _window.Close();
=======
                _parentViewModel.LoadEmployees();
                _window.Close();
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
>>>>>>> origin/Develop
        }

        private void Cancel(object parameter)
        {
            _window.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
