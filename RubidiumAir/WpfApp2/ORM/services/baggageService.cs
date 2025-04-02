using System;
using System.Collections.Generic;

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
    public void RegisterBaggage(string baggageId, string passengerId, double weight, int flightId)
    {
        if (_flightRepo.GetById(flightId) == null)
            throw new KeyNotFoundException("Рейс не существует");

        var baggage = new Baggage
        {
            Id = baggageId,
            PassengerId = passengerId,
            Weight = weight,
            Status = "Registered",
            FlightId = flightId
        };

        _baggageRepo.Add(baggage);
    }

    // Удаление багажа
    public void RemoveBaggage(string baggageId)
    {
        _baggageRepo.Delete(baggageId);
    }

    // Обновление статуса багажа
    public void UpdateBaggageStatus(string baggageId, string newStatus)
    {
        _baggageRepo.UpdateStatus(baggageId, newStatus);
    }

    // Список багажа по рейсу
    public List<Baggage> GetBaggageForFlight(int flightId)
    {
        return _baggageRepo.GetByFlightId(flightId);
    }

    // Дополнительные методы
    public Baggage GetBaggage(string id) => _baggageRepo.GetById(id);
    
    public List<Baggage> GetBaggageByPassenger(string passengerId) 
        => _baggageRepo.GetByPassengerId(passengerId);
}