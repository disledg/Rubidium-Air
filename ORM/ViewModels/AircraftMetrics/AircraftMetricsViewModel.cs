using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;

namespace Rubidium
{
    public class AircraftMetricsViewModel : INotifyPropertyChanged
    {
        private readonly AircraftMetricsService _metricsService;
        private readonly AircraftService _aircraftService;
        private ObservableCollection<AircraftMetric> _metrics;
        private ObservableCollection<Aircraft> _aircraft;
        private Aircraft _selectedAircraft;
        private AircraftMetric _selectedMetrics;
        private DateTime _startDate;
        private DateTime _endDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public SeriesCollection FuelConsumptionSeries { get; set; }
        public SeriesCollection LoadSeries { get; set; }
        public SeriesCollection ResourcesSeries { get; set; }
        public string[] Labels { get; set; }

        public Aircraft SelectedAircraft
        {
            get => _selectedAircraft;
            set
            {
                _selectedAircraft = value;
                OnPropertyChanged(nameof(SelectedAircraft));
                LoadMetrics();
                UpdateCharts();
            }
        }

        public AircraftMetric SelectedMetrics
        {
            get => _selectedMetrics;
            set
            {
                _selectedMetrics = value;
                OnPropertyChanged(nameof(SelectedMetrics));
                ((RelayCommand)DelMetricsCommand)?.RaiseCanExecuteChanged();
                ((RelayCommand)UpdMetricsCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<AircraftMetric> Metrics
        {
            get => _metrics;
            set
            {
                _metrics = value;
                OnPropertyChanged(nameof(Metrics));
            }
        }

        public ObservableCollection<Aircraft> Aircraft
        {
            get => _aircraft;
            set
            {
                _aircraft = value;
                OnPropertyChanged(nameof(Aircraft));
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                LoadMetrics();
                UpdateCharts();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                LoadMetrics();
                UpdateCharts();
            }
        }

        public ICommand AddMetricsCommand { get; }
        public ICommand DelMetricsCommand { get; }
        public ICommand UpdMetricsCommand { get; }

        public AircraftMetricsViewModel(AircraftMetricsService metricsService, AircraftService aircraftService)
        {
            _metricsService = metricsService ?? throw new ArgumentNullException(nameof(metricsService));
            _aircraftService = aircraftService ?? throw new ArgumentNullException(nameof(aircraftService));
            
            Metrics = new ObservableCollection<AircraftMetric>();
            Aircraft = new ObservableCollection<Aircraft>();

            StartDate = DateTime.Now.AddDays(-30);
            EndDate = DateTime.Now;

            AddMetricsCommand = new RelayCommand(AddMetrics, param => SelectedAircraft != null);
            DelMetricsCommand = new RelayCommand(DeleteMetrics, param => SelectedMetrics != null);
            UpdMetricsCommand = new RelayCommand(UpdateMetrics, param => SelectedMetrics != null);

            InitializeCharts();
            LoadAircraft();
        }

        private void InitializeCharts()
        {
            FuelConsumptionSeries = new SeriesCollection();
            LoadSeries = new SeriesCollection();
            ResourcesSeries = new SeriesCollection();
        }

        private void LoadAircraft()
        {
            Aircraft.Clear();
            var aircraftList = _aircraftService.GetAllAircraft().ToList();
            foreach (var aircraft in aircraftList)
            {
                Aircraft.Add(aircraft);
            }
            
            if (aircraftList.Any())
            {
                SelectedAircraft = aircraftList.First();
            }
        }

        private void LoadMetrics()
        {
            if (SelectedAircraft == null) return;

            Metrics.Clear();
            var metrics = _metricsService.GetMetricsByDateRange(SelectedAircraft.Id, StartDate, EndDate);
            foreach (var metric in metrics)
            {
                Metrics.Add(metric);
            }
        }

        private void UpdateCharts()
        {
            if (SelectedAircraft == null || !Metrics.Any()) return;

            var orderedMetrics = Metrics.OrderBy(m => m.date).ToList();
            Labels = orderedMetrics.Select(m => m.date.ToString("dd/MM")).ToArray();

            // Обновление графика расхода топлива
            FuelConsumptionSeries.Clear();
            FuelConsumptionSeries.Add(new LineSeries
            {
                Title = "Расход топлива",
                Values = new ChartValues<decimal>(orderedMetrics.Select(m => m.fuel_consumption))
            });

            // Обновление графика загрузки
            LoadSeries.Clear();
            LoadSeries.Add(new LineSeries
            {
                Title = "Загрузка пассажирами",
                Values = new ChartValues<int>(orderedMetrics.Select(m => m.passenger_load))
            });
            LoadSeries.Add(new LineSeries
            {
                Title = "Загрузка грузом",
                Values = new ChartValues<decimal>(orderedMetrics.Select(m => m.cargo_load))
            });

            // Обновление графика ресурсов
            ResourcesSeries.Clear();
            ResourcesSeries.Add(new LineSeries
            {
                Title = "Уровень воды",
                Values = new ChartValues<decimal>(orderedMetrics.Select(m => m.water_level))
            });
            ResourcesSeries.Add(new LineSeries
            {
                Title = "Уровень масла",
                Values = new ChartValues<decimal>(orderedMetrics.Select(m => m.oil_level))
            });
            ResourcesSeries.Add(new LineSeries
            {
                Title = "Техническое состояние",
                Values = new ChartValues<decimal>(orderedMetrics.Select(m => m.technical_condition))
            });

            OnPropertyChanged(nameof(FuelConsumptionSeries));
            OnPropertyChanged(nameof(LoadSeries));
            OnPropertyChanged(nameof(ResourcesSeries));
            OnPropertyChanged(nameof(Labels));
        }

        private void AddMetrics(object parameter)
        {
            if (SelectedAircraft != null)
            {
                var window = new AddAircraftMetricsView();
                var viewModel = new AddAircraftMetricsViewModel(_metricsService, SelectedAircraft.Id, window);
                window.DataContext = viewModel;
                window.ShowDialog();

                LoadMetrics();
                UpdateCharts();
            }
        }

        private void DeleteMetrics(object parameter)
        {
            if (SelectedMetrics != null)
            {
                try
                {
                    _metricsService.DeleteMetrics(SelectedMetrics.Id);
                    LoadMetrics();
                    UpdateCharts();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private void UpdateMetrics(object parameter)
        {
            if (SelectedMetrics != null)
            {
                var window = new EditAircraftMetricsView();
                var viewModel = new EditAircraftMetricsViewModel(SelectedMetrics, _metricsService, window);
                window.DataContext = viewModel;
                window.ShowDialog();

                LoadMetrics();
                UpdateCharts();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 