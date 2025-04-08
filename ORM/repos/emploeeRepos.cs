using Microsoft.EntityFrameworkCore;
using Rubidium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EmployeeRepo
{
    private readonly AirportDataBaseContext _db;

    public EmployeeRepo(AirportDataBaseContext db)
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

        if (string.IsNullOrEmpty(employee.first_name) & string.IsNullOrEmpty(employee.last_name))
            throw new ArgumentException("ФИО сотрудника обязательно");

        if (string.IsNullOrEmpty(employee.position))
            throw new ArgumentException("Должность обязательна");

        _db.Employees.Add(employee);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var employee = _db.Employees
            .Include(e => e.Flights)
            .FirstOrDefault(e => e.employee_id == id);

        if (employee == null) return;

        if (employee.Flights.Any())
            throw new InvalidOperationException("Нельзя удалить сотрудника, привязанного к рейсам");

        _db.Employees.Remove(employee);
        _db.SaveChanges();
    }

    public void Update(Employee updatedEmployee)
    {
        ValidateEntity(updatedEmployee);

        var employee = _db.Employees.Find(updatedEmployee.employee_id);
        if (employee == null)
            throw new KeyNotFoundException("Сотрудник не найден");

        employee.first_name = updatedEmployee.first_name;
        employee.last_name = updatedEmployee.last_name;
        employee.position = updatedEmployee.position;
        employee.contact_info = updatedEmployee.contact_info;

        _db.SaveChanges();
    }

    public Employee GetById(int id)
    {
        return _db.Employees.Find(id);
    }

    public List<Employee> SearchByName(string name)
    {
        return _db.Employees.Where(e => e.first_name.Contains(name)).ToList();
    }

    public List<Employee> GetByPosition(string position)
    {
        return _db.Employees.Where(e => e.position == position).ToList();
    }

    public List<Employee> GetAll()
    {
        return _db.Employees.ToList();
    }

    public void AssignToFlight(int employeeId, int flightId)
    {
        var transaction = _db.Database.BeginTransaction();
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