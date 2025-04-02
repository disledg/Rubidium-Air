using System;
using System.Collections.Generic;
using System.Linq;

public class FlightService
{
    private readonly FlightRepo _flightRepo;
    private readonly BaggageRepo _baggageRepo;

    public FlightService(FlightRepo flightRepo, BaggageRepo baggageRepo)
    {
        _flightRepo = flightRepo;
        _baggageRepo = baggageRepo;
    }

    // Создание рейса
    public void CreateFlight(string number, string destination, DateTime departure, DateTime arrival, string status)
    {
        if (departure >= arrival)
            throw new ArgumentException("Время вылета должно быть раньше прибытия");

        var flight = new Flight
        {
            FlightNumber = number,
            Destination = destination,
            DepartureTime = departure,
            ArrivalTime = arrival,
            Status = status
        };

        _flightRepo.Add(flight);
    }

    // Удаление рейса
    public void DeleteFlight(int flightId)
    {
        var baggageCount = _baggageRepo.GetByFlightId(flightId).Count;
        if (baggageCount > 0)
            throw new InvalidOperationException($"Невозможно удалить рейс: привязано {baggageCount} единиц багажа");

        _flightRepo.Delete(flightId);
    }

    // Обновление рейса
    public void UpdateFlight(int id, string newNumber, string newDestination, 
                           DateTime newDeparture, DateTime newArrival, string newStatus)
    {
        var flight = _flightRepo.GetById(id) ?? throw new KeyNotFoundException("Рейс не найден");
        
        flight.FlightNumber = newNumber;
        flight.Destination = newDestination;
        flight.DepartureTime = newDeparture;
        flight.ArrivalTime = newArrival;
        flight.Status = newStatus;

        _flightRepo.Update(flight);
    }

    // Поиск рейсов
    public List<Flight> SearchFlights(string byNumber = null, string byDestination = null)
    {
        var query = _flightRepo.GetAll().AsQueryable();

        if (!string.IsNullOrEmpty(byNumber))
            query = query.Where(f => f.FlightNumber.Contains(byNumber));

        if (!string.IsNullOrEmpty(byDestination))
            query = query.Where(f => f.Destination.Contains(byDestination));

        return query.ToList();
    }

    // Дополнительный метод для эпика
    public List<Flight> GetFlightsByStatus(string status) 
        => _flightRepo.GetByStatus(status);
}