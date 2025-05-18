using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubidium
{
    public class AircraftService
    {
        private readonly AircraftRepo _aircraftRepo;

        public AircraftService(AircraftRepo aircraftRepo)
        {
            _aircraftRepo = aircraftRepo;
        }

        public void AddAircraft(string registrationNumber, string model, int yearOfManufacture, 
                              int passengerCapacity, decimal cargoCapacity)
        {
            var aircraft = new Aircraft
            {
                registration_number = registrationNumber,
                model = model,
                year_of_manufacture = yearOfManufacture,
                passenger_capacity = passengerCapacity,
                cargo_capacity = cargoCapacity
            };

            _aircraftRepo.Add(aircraft);
        }

        public void UpdateAircraft(int id, string registrationNumber, string model, 
                                 int yearOfManufacture, int passengerCapacity, decimal cargoCapacity)
        {
            var aircraft = _aircraftRepo.GetById(id) ?? throw new KeyNotFoundException("Самолет не найден");

            aircraft.registration_number = registrationNumber;
            aircraft.model = model;
            aircraft.year_of_manufacture = yearOfManufacture;
            aircraft.passenger_capacity = passengerCapacity;
            aircraft.cargo_capacity = cargoCapacity;

            _aircraftRepo.Update(aircraft);
        }

        public void DeleteAircraft(int aircraftId)
        {
            _aircraftRepo.Delete(aircraftId);
        }

        public Aircraft GetAircraft(int id) => _aircraftRepo.GetById(id);

        public Aircraft GetAircraftByRegistration(string registrationNumber)
            => _aircraftRepo.GetByRegistrationNumber(registrationNumber);

        public List<Aircraft> GetAircraftByModel(string model)
            => _aircraftRepo.GetByModel(model);

        public IQueryable<Aircraft> GetAllAircraft() => _aircraftRepo.GetAll();
    }
} 