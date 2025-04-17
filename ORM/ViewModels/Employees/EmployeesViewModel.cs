using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Rubidium;

namespace Rubidium
{
    public class EmployeesViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeService _employeeService;

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
                // Обновляем состояние команд, которые зависят от выбранного сотрудника
                ((RelayCommand)DelEmployeeCommand).RaiseCanExecuteChanged();
                ((RelayCommand)UpdEmployeeCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddEmployeeCommand { get; }
        public ICommand DelEmployeeCommand { get; }
        public ICommand UpdEmployeeCommand { get; }

        private void AddEmployee(object parameter)
        {
            // Логика добавления нового сотрудника
            var addWindow = new AddEmployeesView();
            var addViewModel = new AddEmployeeViewModel(this,_employeeService, addWindow);
            addWindow.DataContext = addViewModel;
            addWindow.Owner = Application.Current.MainWindow; // Делаем главное окно владельцем
            addWindow.ShowDialog();
        }

        private void DelEmployee(object parameter)
        {
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
        }

        private bool CanDelEmployee(object parameter) => SelectedEmployee != null;

        private void UpdEmployee(object parameter)
        {
            if (SelectedEmployee != null)
            {
                var editWindow = new EditEmployeeView();
                var editViewModel = new EditEmployeeViewModel(SelectedEmployee, _employeeService, editWindow);
                editWindow.DataContext = editViewModel;
                editWindow.Owner = Application.Current.MainWindow;
                editWindow.ShowDialog();

                LoadEmployees(); // Обновляем список после редактирования
            }
        }

        private bool CanUpdEmployee(object parameter) => SelectedEmployee != null;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
