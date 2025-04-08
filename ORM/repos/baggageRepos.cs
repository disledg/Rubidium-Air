using Microsoft.EntityFrameworkCore;
using Rubidium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BaggageRepo
{
    private readonly AirportDataBaseContext _db;
    private static readonly string[] ValidStatuses = { "Проверен", "Загружен", "Транспортирован", "Доставлен" };

    public BaggageRepo(AirportDataBaseContext db)
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

        if (_db.Flights.Find(baggage.flight_id) == null)
            throw new KeyNotFoundException("Рейс не существует");

        if (baggage.passenger_number == 0 & string.IsNullOrEmpty(baggage.passenger_name) & string.IsNullOrEmpty(baggage.passenger_sername))
            throw new ArgumentException("ФИО и номер пасспорта пассажира обязательно");

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

        var baggage = _db.Baggages.Find(updatedBaggage.baggage_id);
        if (baggage == null)
            throw new KeyNotFoundException("Багаж не найден");

        baggage.weight = updatedBaggage.weight;
        baggage.passenger_number = updatedBaggage.passenger_number;
        baggage.passenger_name = updatedBaggage.passenger_name;
        baggage.passenger_sername = updatedBaggage.passenger_sername;
        baggage.flight_id = updatedBaggage.flight_id;
        baggage.status = updatedBaggage.status;

        _db.SaveChanges();
    }

    public void UpdateStatus(string id, string newStatus)
    {
        if (!ValidStatuses.Contains(newStatus))
            throw new ArgumentException("Недопустимый статус багажа");

        var baggage = _db.Baggages.Find(id);
        if (baggage == null)
            throw new KeyNotFoundException("Багаж не найден");

        baggage.status = newStatus;
        _db.SaveChanges();
    }

    public Baggage GetById(string id)
    {
        return _db.Baggages.Find(id);
    }

    public List<Baggage> GetByFlightId(int flightId)
    {
        return _db.Baggages.Where(b => b.flight_id == flightId).ToList();
    }
    public List<Baggage> GetByPassengerNumber(int passengerNumber)
    {
        return _db.Baggages.Where(b => b.passenger_number == passengerNumber).ToList();
    }
    public List<Baggage> GetAll()
    {
        return _db.Baggages.ToList();
    }
}