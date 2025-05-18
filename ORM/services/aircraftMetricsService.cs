using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubidium
{
    public class AircraftMetricsService
    {
        private readonly AircraftMetricsRepo _metricsRepo;
        private readonly AircraftRepo _aircraftRepo;

        public AircraftMetricsService(AircraftMetricsRepo metricsRepo, AircraftRepo aircraftRepo)
        {
            _metricsRepo = metricsRepo;
            _aircraftRepo = aircraftRepo;
        }

        public void AddMetrics(int aircraftId, decimal fuelConsumption, int passengerLoad,
                             decimal cargoLoad, decimal waterLevel, decimal oilLevel,
                             decimal technicalCondition)
        {
            if (_aircraftRepo.GetById(aircraftId) == null)
                throw new KeyNotFoundException("Самолет не найден");

            var metrics = new AircraftMetric
            {
                aircraft_id = aircraftId,
                date = DateTime.Now,
                fuel_consumption = fuelConsumption,
                passenger_load = passengerLoad,
                cargo_load = cargoLoad,
                water_level = waterLevel,
                oil_level = oilLevel,
                technical_condition = technicalCondition
            };

            _metricsRepo.Add(metrics);
        }

        public void UpdateMetrics(int id, decimal fuelConsumption, int passengerLoad,
                                decimal cargoLoad, decimal waterLevel, decimal oilLevel,
                                decimal technicalCondition)
        {
            var metrics = _metricsRepo.GetById(id) ?? throw new KeyNotFoundException("Метрики не найдены");

            metrics.fuel_consumption = fuelConsumption;
            metrics.passenger_load = passengerLoad;
            metrics.cargo_load = cargoLoad;
            metrics.water_level = waterLevel;
            metrics.oil_level = oilLevel;
            metrics.technical_condition = technicalCondition;

            _metricsRepo.Update(metrics);
        }

        public void DeleteMetrics(int metricsId)
        {
            _metricsRepo.Delete(metricsId);
        }

        public List<AircraftMetric> GetMetricsByAircraft(int aircraftId)
            => _metricsRepo.GetByAircraftId(aircraftId);

        public List<AircraftMetric> GetMetricsByDateRange(int aircraftId, DateTime startDate, DateTime endDate)
            => _metricsRepo.GetByDateRange(aircraftId, startDate, endDate);

        public AircraftMetric GetLatestMetrics(int aircraftId)
            => _metricsRepo.GetLatestMetrics(aircraftId);

        public IQueryable<AircraftMetric> GetAllMetrics() => _metricsRepo.GetAll();
    }
} 