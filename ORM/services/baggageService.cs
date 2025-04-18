using System;
using System.Collections.Generic;
using System.Linq;
using Rubidium;
namespace Rubidium
{
    public class BaggageService
    {
        private readonly BaggageRepo _baggageRepo;
        private readonly FlightRepo _flightRepo;

        public BaggageService(BaggageRepo baggageRepo, FlightRepo flightRepo)
        {
            _baggageRepo = baggageRepo;
            _flightRepo = flightRepo;
        }

        // Добавление багажа
        public void RegisterBaggage(int baggageId, string passengerSername, decimal weight, int flightId, string passengerName, int passengerNumber)
        {
            if (_flightRepo.GetById(flightId) == null)
                throw new KeyNotFoundException("Рейс не существует");

            var baggage = new Baggage()
            {
                Id = baggageId,
                passenger_number = passengerNumber,
                passenger_sername = passengerSername,
                passenger_name = passengerName,
                weight = weight,
                status = "Registered",
                flight_id = flightId
            };

            _baggageRepo.Add(baggage);
        }

        // Удаление багажа
        public void RemoveBaggage(int baggageId)
        {
            _baggageRepo.Delete(baggageId);
        }

        // Обновление статуса багажа
        public void UpdateBaggageStatus(int baggageId, string newStatus)
        {
            _baggageRepo.UpdateStatus(baggageId, newStatus);
        }

        // Список багажа по рейсу
        public List<Baggage> GetBaggageForFlight(int flightId)
        {
            return _baggageRepo.GetByFlightId(flightId);
        }

        // Дополнительные методы
        public Baggage GetBaggage(int id) => _baggageRepo.GetById(id);

        public List<Baggage> GetBaggageByPassenger(int passengerNumber)
            => _baggageRepo.GetByPassengerNumber(passengerNumber);
        public IQueryable<Baggage> GetAllBaggage() => _baggageRepo.GetAll();
    }
}
