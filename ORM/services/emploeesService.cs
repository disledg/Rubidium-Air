using System;
using System.Collections.Generic;
using Rubidium;

public class EmployeeService
{
    private readonly EmployeeRepo _employeeRepo;
    private readonly FlightRepo _flightRepo;

    public EmployeeService(EmployeeRepo employeeRepo, FlightRepo flightRepo)
    {
        _employeeRepo = employeeRepo;
        _flightRepo = flightRepo;
    }

    // Добавление сотрудника
    public void AddEmployee(string first_name, string last_name, string position, string contact)
    {
        var employee = new Employee
        {
            first_name = first_name,
            last_name = last_name,
            position = position,
            contact_info = contact,
            
        };

        _employeeRepo.Add(employee);
    }

    // Удаление сотрудника
    public void RemoveEmployee(int employeeId)
    {
        _employeeRepo.Delete(employeeId);
    }

    // Обновление информации
    public void UpdateEmployee(int id, string newName,string newSername, string newPosition, string newContact)
    {
        var employee = _employeeRepo.GetById(id) ?? throw new KeyNotFoundException("Сотрудник не найден");

        employee.first_name = newName;
        employee.last_name = newSername;
        employee.position = newPosition;
        employee.contact_info = newContact;

        _employeeRepo.Update(employee);
    }

    // Прикрепление к рейсу
    public void AssignToFlight(int employeeId, int flightId)
    {
        if (_flightRepo.GetById(flightId) == null)
            throw new KeyNotFoundException("Рейс не найден");

        _employeeRepo.AssignToFlight(employeeId, flightId);
    }

    // Дополнительные методы
    public List<Employee> SearchEmployees(string name)
        => _employeeRepo.SearchByName(name);

    public List<Employee> GetCrewForFlight(int flightId)
        => _flightRepo.GetCrew(flightId);
}