using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
<<<<<<< HEAD
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
=======
using System.Windows;
using System.Windows.Input;
>>>>>>> origin/Develop
using Rubidium;

namespace Rubidium
{
    public class EmployeesViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeService _employeeService;
<<<<<<< HEAD

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Employee> _employees;
        private Employee _selectedEmployee;

        public EmployeesViewModel(EmployeeService employeeService)
        {
            // Инициализация коллекции сотрудников (можно заменить на загрузку из БД)
            _employeeService = employeeService;
            LoadEmployees();
            // Инициализация команд
            AddEmployeeCommand = new RelayCommand(AddEmployee);
            DelEmployeeCommand = new RelayCommand(DelEmployee, CanDelEmployee);
            UpdEmployeeCommand = new RelayCommand(UpdEmployee, CanUpdEmployee);
=======
        private ObservableCollection<Employee> _employees;
        private Employee _selectedEmployee;

        public event PropertyChangedEventHandler PropertyChanged;

        public EmployeesViewModel(EmployeeService employeeService)
        {
            try
            {
                _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
                LoadEmployees();

                AddEmployeeCommand = new RelayCommand(AddEmployee);
                DelEmployeeCommand = new RelayCommand(DelEmployee, CanDelEmployee);
                UpdEmployeeCommand = new RelayCommand(UpdEmployee, CanUpdEmployee);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации ViewModel: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                // Инициализация пустой коллекции в случае ошибки
                _employees = new ObservableCollection<Employee>();
            }
>>>>>>> origin/Develop
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
<<<<<<< HEAD
                // Обновляем состояние команд, которые зависят от выбранного сотрудника
                ((RelayCommand)DelEmployeeCommand).RaiseCanExecuteChanged();
                ((RelayCommand)UpdEmployeeCommand).RaiseCanExecuteChanged();
=======
                ((RelayCommand)DelEmployeeCommand)?.RaiseCanExecuteChanged();
                ((RelayCommand)UpdEmployeeCommand)?.RaiseCanExecuteChanged();
>>>>>>> origin/Develop
            }
        }

        public ICommand AddEmployeeCommand { get; }
        public ICommand DelEmployeeCommand { get; }
        public ICommand UpdEmployeeCommand { get; }

        private void AddEmployee(object parameter)
        {
<<<<<<< HEAD
            // Логика добавления нового сотрудника
            var addWindow = new AddEmployeesView();
            var addViewModel = new AddEmployeeViewModel(this,_employeeService, addWindow);
            addWindow.DataContext = addViewModel;
            addWindow.Owner = Application.Current.MainWindow; // Делаем главное окно владельцем
            addWindow.ShowDialog();
=======
            try
            {
                var addWindow = new AddEmployeesView();
                var addViewModel = new AddEmployeeViewModel(this, _employeeService, addWindow);
                addWindow.DataContext = addViewModel;
                addWindow.Owner = Application.Current.MainWindow;
                addWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна добавления сотрудника: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
>>>>>>> origin/Develop
        }

        private void DelEmployee(object parameter)
        {
<<<<<<< HEAD
            if (SelectedEmployee != null)
            {
                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
                _employeeService.RemoveEmployee(SelectedEmployee.Id);
            }
        }
        public void LoadEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            _employees = new ObservableCollection<Employee>(employees);
=======
            if (SelectedEmployee == null) return;

            try
            {
                var employeeToDelete = SelectedEmployee;
                _employeeService.RemoveEmployee(employeeToDelete.Id);
                Employees.Remove(employeeToDelete);
                SelectedEmployee = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadEmployees()
        {
            try
            {
                var employees = _employeeService.GetAllEmployees();
                Employees = new ObservableCollection<Employee>(employees);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка сотрудников: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Employees = new ObservableCollection<Employee>();
            }
>>>>>>> origin/Develop
        }

        private bool CanDelEmployee(object parameter) => SelectedEmployee != null;

        private void UpdEmployee(object parameter)
        {
<<<<<<< HEAD
            if (SelectedEmployee != null)
            {
                var editWindow = new EditEmployeeView();
                var editViewModel = new EditEmployeeViewModel(SelectedEmployee, _employeeService, editWindow);
                editWindow.DataContext = editViewModel;
                editWindow.Owner = Application.Current.MainWindow;
                editWindow.ShowDialog();

                LoadEmployees(); // Обновляем список после редактирования
=======
            if (SelectedEmployee == null) return;

            try
            {
                var editWindow = new EditEmployeeView(SelectedEmployee, _employeeService);
                editWindow.Owner = Application.Current.MainWindow;
                editWindow.ShowDialog();

                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании сотрудника: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
>>>>>>> origin/Develop
            }
        }

        private bool CanUpdEmployee(object parameter) => SelectedEmployee != null;

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
