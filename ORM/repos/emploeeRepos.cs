using Microsoft.EntityFrameworkCore;
using Rubidium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubidium
{
    public class EmployeeRepo : Repository<Employee>, IEmployeeRepo
    {
        public EmployeeRepo(AirportDBEntities1 db) : base(db)
        {
        }

        public override void Add(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            if (string.IsNullOrEmpty(employee.first_name) & string.IsNullOrEmpty(employee.last_name))
                throw new ArgumentException("ФИО сотрудника обязательно");

            if (string.IsNullOrEmpty(employee.position))
                throw new ArgumentException("Должность обязательна");

            base.Add(employee);
            Save();
        }

        public override void Delete(int id)
        {
            var db = _context as AirportDBEntities1;
            var employee = db.Employees
                .Include(e => e.Flights)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null) return;

            if (employee.Flights.Any())
                throw new InvalidOperationException("Нельзя удалить сотрудника, привязанного к рейсам");

            base.Delete(id);
            Save();
        }

        public override void Update(Employee updatedEmployee)
        {
            if (updatedEmployee == null) throw new ArgumentNullException(nameof(updatedEmployee));

            var employee = GetById(updatedEmployee.Id);
            if (employee == null)
                throw new KeyNotFoundException("Сотрудник не найден");

            employee.first_name = updatedEmployee.first_name;
            employee.last_name = updatedEmployee.last_name;
            employee.position = updatedEmployee.position;
            employee.contact_info = updatedEmployee.contact_info;

            base.Update(employee);
            Save();
        }

        public List<Employee> SearchByName(string name)
        {
            return _dbSet.Where(e => e.first_name.Contains(name)).ToList();
        }

        public List<Employee> GetByPosition(string position)
        {
            return _dbSet.Where(e => e.position == position).ToList();
        }

        public void AssignToFlight(int employeeId, int flightId)
        {
            var db = _context as AirportDBEntities1;
            var transaction = db.Database.BeginTransaction();
            try
            {
                var employee = db.Employees.Find(employeeId);
                if (employee == null)
                    throw new KeyNotFoundException("Сотрудник не найден");

                var flight = db.Flights.Find(flightId);
                if (flight == null)
                    throw new KeyNotFoundException("Рейс не найден");

                employee.Flights.Add(flight);
                Save();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}