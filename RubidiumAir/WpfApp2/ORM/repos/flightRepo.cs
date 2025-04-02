using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FlightRepo
{
    private readonly AirportDbContext _db;
    private static readonly string[] ValidStatuses = { "Запланировано", "Отложенный", "Отменено", "Отправлен", "Прибыл" };

    public FlightRepo(AirportDbContext db)
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
        
        if (string.IsNullOrEmpty(flight.FlightNumber))
            throw new ArgumentException("Номер рейса обязателен");

        if (flight.DepartureTime >= flight.ArrivalTime)
            throw new ArgumentException("Время вылета должно быть раньше времени прилёта");

        if (_db.Flights.Any(f => f.FlightNumber == flight.FlightNumber))
            throw new InvalidOperationException("Рейс с таким номером уже существует");

        _db.Flights.Add(flight);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var flight = _db.Flights
            .Include(f => f.Baggages)
            .FirstOrDefault(f => f.Id == id);

        if (flight == null) return;

        if (flight.Baggages.Any())
            throw new InvalidOperationException("Нельзя удалить рейс с привязанным багажом");

        _db.Flights.Remove(flight);
        _db.SaveChanges();
    }

    public void Update(Flight updatedFlight)
    {
        ValidateEntity(updatedFlight);
        
        var flight = _db.Flights.Find(updatedFlight.Id);
        if (flight == null) 
            throw new KeyNotFoundException("Рейс не найден");

        if (!ValidStatuses.Contains(updatedFlight.Status))
            throw new ArgumentException("Недопустимый статус рейса");

        flight.FlightNumber = updatedFlight.FlightNumber;
        flight.Destination = updatedFlight.Destination;
        flight.DepartureTime = updatedFlight.DepartureTime;
        flight.ArrivalTime = updatedFlight.ArrivalTime;
        flight.Status = updatedFlight.Status;

        _db.SaveChanges();
    }

    public Flight GetById(int id) 
    {    
        _db.Flights.Find(id);
    }
    public Flight GetByNumber(string number)
    {
        _db.Flights.FirstOrDefault(f => f.FlightNumber == number);
    }

    public List<Flight> GetByDestination(string destination)
    {
        _db.Flights.Where(f => f.Destination.Contains(destination)).ToList();
    }

    public List<Flight> GetByStatus(string status)
    {
        _db.Flights.Where(f => f.Status == status).ToList();
    }

    public List<Flight> GetAll() => _db.Flights.ToList();

    public List<Employee> GetCrew(int flightId) 
    {
        _db.Flights
            .Include(f => f.Crew)
            .Where(f => f.Id == flightId)
            .SelectMany(f => f.Crew)
            .ToList();
    }
}