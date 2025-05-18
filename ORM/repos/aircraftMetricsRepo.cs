using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubidium
{
    public class AircraftMetricsRepo : Repository<AircraftMetric>, IAircraftMetricsRepo
    {
        public AircraftMetricsRepo(AirportDBEntities1 db) : base(db)
        {
        }

        public override void Add(AircraftMetric metrics)
        {
            if (metrics == null) throw new ArgumentNullException(nameof(metrics));

            var db = _context as AirportDBEntities1;
            if (db.Aircraft.Find(metrics.aircraft_id) == null)
                throw new KeyNotFoundException("Самолет не найден");

            if (metrics.technical_condition < 0 || metrics.technical_condition > 100)
                throw new ArgumentException("Техническое состояние должно быть в диапазоне от 0 до 100");

            base.Add(metrics);
            Save();
        }

        public override void Update(AircraftMetric updatedMetrics)
        {
            if (updatedMetrics == null) throw new ArgumentNullException(nameof(updatedMetrics));

            var metrics = GetById(updatedMetrics.Id);
            if (metrics == null)
                throw new KeyNotFoundException("Метрики не найдены");

            metrics.date = updatedMetrics.date;
            metrics.fuel_consumption = updatedMetrics.fuel_consumption;
            metrics.passenger_load = updatedMetrics.passenger_load;
            metrics.cargo_load = updatedMetrics.cargo_load;
            metrics.water_level = updatedMetrics.water_level;
            metrics.oil_level = updatedMetrics.oil_level;
            metrics.technical_condition = updatedMetrics.technical_condition;

            base.Update(metrics);
            Save();
        }

        public List<AircraftMetric> GetByAircraftId(int aircraftId)
        {
            return _dbSet.Where(m => m.aircraft_id == aircraftId)
                        .OrderByDescending(m => m.date)
                        .ToList();
        }

        public List<AircraftMetric> GetByDateRange(int aircraftId, DateTime startDate, DateTime endDate)
        {
            return _dbSet.Where(m => m.aircraft_id == aircraftId && 
                                   m.date >= startDate && 
                                   m.date <= endDate)
                        .OrderBy(m => m.date)
                        .ToList();
        }

        public AircraftMetric GetLatestMetrics(int aircraftId)
        {
            return _dbSet.Where(m => m.aircraft_id == aircraftId)
                        .OrderByDescending(m => m.date)
                        .FirstOrDefault();
        }
    }
} 