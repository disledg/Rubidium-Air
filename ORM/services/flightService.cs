using System;
using System.Collections.Generic;
using System.Linq;
using Rubidium;

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
            flight_number = number,
            destination = destination,
            departure_time = departure,
            arrival_time = arrival,
            status = status
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

        flight.flight_number = newNumber;
        flight.destination = newDestination;
        flight.departure_time = newDeparture;
        flight.arrival_time = newArrival;
        flight.status = newStatus;

        _flightRepo.Update(flight);
    }

    // Поиск рейсов
    public List<Flight> SearchFlights(string byNumber = null, string byDestination = null)
    {
        var query = _flightRepo.GetAll().AsQueryable();

        if (!string.IsNullOrEmpty(byNumber))
            query = query.Where(f => f.flight_number.Contains(byNumber));

        if (!string.IsNullOrEmpty(byDestination))
            query = query.Where(f => f.destination.Contains(byDestination));

        return query.ToList();
    }

    // Дополнительный метод для эпика
    public List<Flight> GetFlightsByStatus(string status)
        => _flightRepo.GetByStatus(status);
    public IQueryable<Flight> GetAllFlights() => _flightRepo.GetAll();
}