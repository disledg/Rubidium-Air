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
<<<<<<< HEAD
    public class EditEmployeeViewModel : INotifyPropertyChanged
    {
        private readonly Employee _originalEmployee;
        private readonly EmployeeService _employeeService;
        private readonly Window _window;
=======
    internal class EditEmployeeViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeService _employeeService;
        private readonly Window _window;
        private Employee _originalemployee;
        private readonly FlightRepo _flightRepo;
        private int _employeeId;
        private string _lastName;
        private string _firstName;
        private string _position;
        private string _contactInfo;

        public event PropertyChangedEventHandler PropertyChanged;
>>>>>>> origin/Develop

        public EditEmployeeViewModel(Employee employee,
                                  EmployeeService employeeService,
                                  Window window)
        {
<<<<<<< HEAD
            _originalEmployee = employee;
            _employeeService = employeeService;
            _window = window;

            // Инициализируем свойства текущими значениями
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
            _employeeService.UpdateEmployee(
                _originalEmployee.Id,
                FirstName,
                LastName,
                Position,
                ContactInfo);

            _window.Close();
=======
            _employeeService = employeeService;
            _window = window;
            _originalemployee = employee;

            // Заполняем свойства данными из переданного рейса
            _employeeId = employee.Id;
            _lastName = employee.last_name;
            _firstName = employee.first_name;
            _position = employee.position;
            _contactInfo = employee.contact_info;

            // Инициализация команд
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Свойства для привязки в UI

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }

        public string ContactInfo
        {
            get { return _contactInfo; }
            set
            {
                if (_contactInfo != value)
                {
                    _contactInfo = value;
                    OnPropertyChanged(nameof(ContactInfo));
                }
            }
        }

        // Команды
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private bool CanSave(object parameter)
        {
            // Валидация данных
            return !string.IsNullOrWhiteSpace(LastName)
                && !string.IsNullOrWhiteSpace(FirstName)
                && !string.IsNullOrWhiteSpace(Position)
                && !string.IsNullOrWhiteSpace(ContactInfo);
        }

        private void Save(object parameter)
        {
            try
            {
                // Вызываем метод сервиса для обновления рейса с правильными параметрами
                _employeeService.UpdateEmployee(
                    _employeeId,
                    LastName,
                    FirstName,
                    Position,
                    ContactInfo
                );

                // Закрываем окно после успешного сохранения
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
>>>>>>> origin/Develop
        }

        private void Cancel(object parameter)
        {
<<<<<<< HEAD
            _window.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
=======
            // Просто закрываем окно
            _window.Close();
        }

        // Метод для уведомления об изменении свойств
        protected void OnPropertyChanged(string propertyName)
>>>>>>> origin/Develop
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
