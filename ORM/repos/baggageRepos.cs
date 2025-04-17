using Microsoft.EntityFrameworkCore;
using Rubidium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubidium
{
    public class BaggageRepo : Repository<Baggage>, IBaggageRepo
    {
        private static readonly string[] ValidStatuses = { "Проверен", "Загружен", "Транспортирован", "Доставлен" };

        public BaggageRepo(AirportDBEntities1 db) : base(db)
        {
        }

        public override void Add(Baggage baggage)
        {
            if (baggage == null) throw new ArgumentNullException(nameof(baggage));

            var db = _context as AirportDBEntities1;
            if (db.Flights.Find(baggage.flight_id) == null)
                throw new KeyNotFoundException("Рейс не существует");

            if (baggage.passenger_number == 0 & string.IsNullOrEmpty(baggage.passenger_name) & string.IsNullOrEmpty(baggage.passenger_sername))
                throw new ArgumentException("ФИО и номер пасспорта пассажира обязательно");

            base.Add(baggage);
        }

        public override void Update(Baggage updatedBaggage)
        {
            if (updatedBaggage == null) throw new ArgumentNullException(nameof(updatedBaggage));

            var baggage = GetById(updatedBaggage.Id);
            if (baggage == null)
                throw new KeyNotFoundException("Багаж не найден");

            baggage.weight = updatedBaggage.weight;
            baggage.passenger_number = updatedBaggage.passenger_number;
            baggage.passenger_name = updatedBaggage.passenger_name;
            baggage.passenger_sername = updatedBaggage.passenger_sername;
            baggage.flight_id = updatedBaggage.flight_id;
            baggage.status = updatedBaggage.status;

            base.Update(baggage);
        }

        public void UpdateStatus(int id, string newStatus)
        {
            if (!ValidStatuses.Contains(newStatus))
                throw new ArgumentException("Недопустимый статус багажа");

            var baggage = GetById(id);
            if (baggage == null)
                throw new KeyNotFoundException("Багаж не найден");

            baggage.status = newStatus;
            Save();
        }

        public List<Baggage> GetByFlightId(int flightId)
        {
            return _dbSet.Where(b => b.flight_id == flightId).ToList();
        }

        public List<Baggage> GetByPassengerNumber(int passengerNumber)
        {
            return _dbSet.Where(b => b.passenger_number == passengerNumber).ToList();
        }
    }
}