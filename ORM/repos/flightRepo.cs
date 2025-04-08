using Microsoft.EntityFrameworkCore;
using Rubidium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;

public class FlightRepo
{
    private readonly AirportDataBaseContext _db;
    private static readonly string[] ValidStatuses = { "Запланировано", "Отложенный", "Отменено", "Отправлен", "Прибыл" };

    public FlightRepo(AirportDataBaseContext db)
    {
        _db = db;
    }

    private void ValidateEntity<T>(T entity) where T : class
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
    }

    public void Add(Flight flight)
    {
        ValidateEntity(flight);

        if (string.IsNullOrEmpty(flight.flight_number))
            throw new ArgumentException("Номер рейса обязателен");

        //if (flight.departure_time >= flight.arrival_time)
        //    throw new ArgumentException("Время вылета должно быть раньше времени прилёта");

        if (_db.Flights.Any(f => f.flight_number == flight.flight_number))
            throw new InvalidOperationException("Рейс с таким номером уже существует");

        _db.Flights.Add(flight);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var flight = _db.Flights
            .Include(f => f.Baggages)
            .FirstOrDefault(f => f.flight_id == id);

        if (flight == null) return;

        if (flight.Baggages.Any())
            throw new InvalidOperationException("Нельзя удалить рейс с привязанным багажом");

        _db.Flights.Remove(flight);
        _db.SaveChanges();
    }

    public void Update(Flight updatedFlight)
    {
        ValidateEntity(updatedFlight);

        var flight = _db.Flights.Find(updatedFlight.flight_id);
        if (flight == null)
            throw new KeyNotFoundException("Рейс не найден");

        if (!ValidStatuses.Contains(updatedFlight.status))
            throw new ArgumentException("Недопустимый статус рейса");

        flight.flight_number = updatedFlight.flight_number;
        flight.destination = updatedFlight.destination;
        flight.departure_time = updatedFlight.departure_time;
        flight.arrival_time = updatedFlight.arrival_time;
        flight.status = updatedFlight.status;

        _db.SaveChanges();
    }

    public Flight GetById(int id)
    {
        return _db.Flights.Find(id);
    }
    public Flight GetByNumber(string number)
    {
        return _db.Flights.FirstOrDefault(f => f.flight_number == number);
    }

    public List<Flight> GetByDestination(string destination)
    {
        return _db.Flights.Where(f => f.destination.Contains(destination)).ToList();
    }

    public List<Flight> GetByStatus(string status)
    {
        return _db.Flights.Where(f => f.status == status).ToList();
    }

    public List<Flight> GetAll() => _db.Flights.ToList();

    public List<Employee> GetCrew(int flightId)
    {
        var crew = _db.Flights
            .Where(f => f.flight_id == 1)
            .SelectMany(f => f.Employees)
            .ToList();
        return crew;
    }
}