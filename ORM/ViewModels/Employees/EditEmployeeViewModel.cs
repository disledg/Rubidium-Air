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
    public class EditEmployeeViewModel : INotifyPropertyChanged
    {
        private readonly Employee _originalEmployee;
        private readonly EmployeeService _employeeService;
        private readonly Window _window;

        public EditEmployeeViewModel(Employee employee,
                                  EmployeeService employeeService,
                                  Window window)
        {
            _originalEmployee = employee;
            _employeeService = employeeService;
            _window = window;

            LastName = employee.last_name;
            FirstName = employee.first_name;
            Position = employee.position;
            ContactInfo = employee.contact_info;

            SaveCommand = new RelayCommand(SaveChanges);
            CancelCommand = new RelayCommand(Cancel);
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Position { get; set; }
        public string ContactInfo { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void SaveChanges(object parameter)
        {
            try
            {
                _employeeService.UpdateEmployee(
                _originalEmployee.Id,
                FirstName,
                LastName,
                Position,
                ContactInfo);

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
