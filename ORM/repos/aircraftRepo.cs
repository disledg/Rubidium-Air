using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubidium
{
    public class AircraftRepo : Repository<Aircraft>, IAircraftRepo
    {
        public AircraftRepo(AirportDBEntities1 db) : base(db)
        {
        }

        public override void Add(Aircraft aircraft)
        {
            if (aircraft == null) throw new ArgumentNullException(nameof(aircraft));

            if (string.IsNullOrEmpty(aircraft.registration_number))
                throw new ArgumentException("Бортовой номер обязателен");

            if (string.IsNullOrEmpty(aircraft.model))
                throw new ArgumentException("Модель самолета обязательна");

            var db = _context as AirportDBEntities1;
            if (db.Aircraft.Any(a => a.registration_number == aircraft.registration_number))
                throw new InvalidOperationException("Самолет с таким бортовым номером уже существует");

            base.Add(aircraft);
            Save();
        }

        public override void Update(Aircraft updatedAircraft)
        {
            if (updatedAircraft == null) throw new ArgumentNullException(nameof(updatedAircraft));

            var aircraft = GetById(updatedAircraft.Id);
            if (aircraft == null)
                throw new KeyNotFoundException("Самолет не найден");

            aircraft.registration_number = updatedAircraft.registration_number;
            aircraft.model = updatedAircraft.model;
            aircraft.year_of_manufacture = updatedAircraft.year_of_manufacture;
            aircraft.passenger_capacity = updatedAircraft.passenger_capacity;
            aircraft.cargo_capacity = updatedAircraft.cargo_capacity;

            base.Update(aircraft);
            Save();
        }

        public Aircraft GetByRegistrationNumber(string registrationNumber)
        {
            return _dbSet.FirstOrDefault(a => a.registration_number == registrationNumber);
        }

        public List<Aircraft> GetByModel(string model)
        {
            return _dbSet.Where(a => a.model.Contains(model)).ToList();
        }

        public List<Aircraft> GetByManufactureYear(int year)
        {
            return _dbSet.Where(a => a.year_of_manufacture == year).ToList();
        }
    }
} 