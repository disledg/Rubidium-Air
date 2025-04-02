using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EmployeeRepo
{
    private readonly AirportDbContext _db;

    public EmployeeRepo(AirportDbContext db)
    {
        _db = db;
    }

    private void ValidateEntity<T>(T entity) where T : class
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
    }

    public void Add(Employee employee)
    {
        ValidateEntity(employee);
        
        if (string.IsNullOrEmpty(employee.Name) & string.IsNullOrEmpty(employee.SerName) & string.IsNullOrEmpty(employee.Patronymic))
            throw new ArgumentException("ФИО сотрудника обязательно");

        if (string.IsNullOrEmpty(employee.Position))
            throw new ArgumentException("Должность обязательна");

        _db.Employees.Add(employee);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var employee = _db.Employees
            .Include(e => e.Flights)
            .FirstOrDefault(e => e.Id == id);

        if (employee == null) return;

        if (employee.Flights.Any())
            throw new InvalidOperationException("Нельзя удалить сотрудника, привязанного к рейсам");

        _db.Employees.Remove(employee);
        _db.SaveChanges();
    }

    public void Update(Employee updatedEmployee)
    {
        ValidateEntity(updatedEmployee);
        
        var employee = _db.Employees.Find(updatedEmployee.Id);
        if (employee == null) 
            throw new KeyNotFoundException("Сотрудник не найден");

        employee.FullName = updatedEmployee.FullName;
        employee.Position = updatedEmployee.Position;
        employee.Contact = updatedEmployee.Contact;

        _db.SaveChanges();
    }

    public Employee GetById(int id) 
    {
        _db.Employees.Find(id);
    } 

    public List<Employee> SearchByName(string name) 
    { 
        _db.Employees.Where(e => e.FullName.Contains(name)).ToList();    
    }

    public List<Employee> GetByPosition(string position)
    {
        _db.Employees.Where(e => e.Position == position).ToList();
    }

    public List<Employee> GetAll()
    {
        _db.Employees.ToList();
    }

    public void AssignToFlight(int employeeId, int flightId)
    {
        using var transaction = _db.Database.BeginTransaction();
        try
        {
            var employee = _db.Employees.Find(employeeId); 
            if (employee == null)
                throw new KeyNotFoundException("Сотрудник не найден");

            var flight = _db.Flights.Find(flightId); 
            if (flight == null)
                throw new KeyNotFoundException("Рейс не найден");

            employee.Flights.Add(flight);
            _db.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}