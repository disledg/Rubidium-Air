using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BaggageRepo
{
    private readonly AirportDbContext _db;
    private static readonly string[] ValidStatuses = { "Проверен", "Загружен", "Транспортирован", "Доставлен" };

    public BaggageRepo(AirportDbContext db)
    {
        _db = db;
    }

    private void ValidateEntity<T>(T entity) where T : class
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
    }

    public void Add(Baggage baggage)
    {
        ValidateEntity(baggage);
        
        if (_db.Flights.Find(baggage.FlightId) == null)
            throw new KeyNotFoundException("Рейс не существует");

        if (string.IsNullOrEmpty(baggage.PassengerId))
            throw new ArgumentException("ID пассажира обязательно");

        _db.Baggages.Add(baggage);
        _db.SaveChanges();
    }

    public void Delete(string id)
    {
        var baggage = _db.Baggages.Find(id);
        if (baggage == null) return;

        _db.Baggages.Remove(baggage);
        _db.SaveChanges();
    }

    public void Update(Baggage updatedBaggage)
    {
        ValidateEntity(updatedBaggage);
        
        var baggage = _db.Baggages.Find(updatedBaggage.Id); 
        if (baggage == null)
            throw new KeyNotFoundException("Багаж не найден");

        baggage.Weight = updatedBaggage.Weight;
        baggage.PassengerId = updatedBaggage.PassengerId;
        baggage.FlightId = updatedBaggage.FlightId;
        baggage.Status = updatedBaggage.Status;

        _db.SaveChanges();
    }

    public void UpdateStatus(string id, string newStatus)
    {
        if (!ValidStatuses.Contains(newStatus))
            throw new ArgumentException("Недопустимый статус багажа");

        var baggage = _db.Baggages.Find(id);
        if (baggage == null)
            throw new KeyNotFoundException("Багаж не найден");

        baggage.Status = newStatus;
        _db.SaveChanges();
    }

    public Baggage GetById(string id) 
    {
        _db.Baggages.Find(id);
    }

    public List<Baggage> GetByFlightId(int flightId)
    {
        _db.Baggages.Where(b => b.FlightId == flightId).ToList();
    }
    public List<Baggage> GetByPassengerId(string passengerId)
    {
        _db.Baggages.Where(b => b.PassengerId == passengerId).ToList();
    }
    public List<Baggage> GetAll() 
    { 
        _db.Baggages.ToList();
    }

    public async Task<List<Baggage>> GetAllAsync() 
    {
        await _db.Baggages.ToListAsync();
    } 
}