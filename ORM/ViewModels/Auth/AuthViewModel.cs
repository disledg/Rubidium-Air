using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Rubidium
{
    internal class AuthViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly AuthService _authService;
        private readonly EmployeeService _employeeService;
        private readonly FlightService _flightService;
        private readonly BaggageService _baggageService;
        private readonly NavigationService _navigator;


        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password { get; set; }
        public ICommand AuntificationCommand { get; }
        public AuthViewModel(AuthService authService,
            EmployeeService emploeeService,
            FlightService flightService,
            BaggageService baggageService,
            NavigationService navigator)
        {
            _authService = authService;
            AuntificationCommand = new RelayCommand(_ => Auth("Product"));
            _navigator = navigator;
            _employeeService = emploeeService;
            _flightService = flightService;
            _baggageService = baggageService;
            _authService.Register("root", "toor");
        }
        private void Auth(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                Password = passwordBox.Password;
            }

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show($"Введите логин и пароль");
                return;
            }

            var user = _authService.Login(Username, Password);
            if (user != null)
            {
                var mainContext = new MainViewModel(_navigator, _flightService, _employeeService, _baggageService);

                var mainWindow = new MainWindow
                {
                    DataContext = mainContext
                };
                mainWindow.Show();
                // Закрываем текущее окно
                Application.Current.MainWindow?.Close();
                Application.Current.MainWindow = mainWindow;
            }
            else
            {
                MessageBox.Show("Неверные учетные данные");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
