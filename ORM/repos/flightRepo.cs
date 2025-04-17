using Microsoft.EntityFrameworkCore;
using Rubidium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;

namespace Rubidium
{
    public class FlightRepo : Repository<Flight>, IFlightRepo
    {
        private static readonly string[] ValidStatuses = { "Запланировано", "Отложенный", "Отменено", "Отправлен", "Прибыл" };

        public FlightRepo(AirportDBEntities1 db) : base(db)
        {
        }

        public override void Add(Flight flight)
        {
            if (flight == null) throw new ArgumentNullException(nameof(flight));

            if (string.IsNullOrEmpty(flight.flight_number))
                throw new ArgumentException("Номер рейса обязателен");

            //if (flight.departure_time >= flight.arrival_time)
            //    throw new ArgumentException("Время вылета должно быть раньше времени прилёта");

            var db = _context as AirportDBEntities1;
            if (db.Flights.Any(f => f.flight_number == flight.flight_number))
                throw new InvalidOperationException("Рейс с таким номером уже существует");

            base.Add(flight);
            Save();
        }

        public override void Delete(int id)
        {
            var db = _context as AirportDBEntities1;
            var flight = db.Flights
                .Include(f => f.Baggages)
                .FirstOrDefault(f => f.Id == id);

            if (flight == null) return;

            if (flight.Baggages.Any())
                throw new InvalidOperationException("Нельзя удалить рейс с привязанным багажом");

            base.Delete(id);
            Save();
        }

        public override void Update(Flight updatedFlight)
        {
            if (updatedFlight == null) throw new ArgumentNullException(nameof(updatedFlight));

            var flight = GetById(updatedFlight.Id);
            if (flight == null)
                throw new KeyNotFoundException("Рейс не найден");

            if (!ValidStatuses.Contains(updatedFlight.status))
                throw new ArgumentException("Недопустимый статус рейса");

            flight.flight_number = updatedFlight.flight_number;
            flight.destination = updatedFlight.destination;
            flight.departure_time = updatedFlight.departure_time;
            flight.arrival_time = updatedFlight.arrival_time;
            flight.status = updatedFlight.status;

            base.Update(flight);
            Save();
        }

        public Flight GetByNumber(string number)
        {
            return _dbSet.FirstOrDefault(f => f.flight_number == number);
        }

        public List<Flight> GetByDestination(string destination)
        {
            return _dbSet.Where(f => f.destination.Contains(destination)).ToList();
        }

        public List<Flight> GetByStatus(string status)
        {
            return _dbSet.Where(f => f.status == status).ToList();
        }

        public List<Employee> GetCrew(int flightId)
        {
            var db = _context as AirportDBEntities1;
            return db.Flights
                .Where(f => f.Id == flightId)
                .SelectMany(f => f.Employees)
                .ToList();
        }
    }
}