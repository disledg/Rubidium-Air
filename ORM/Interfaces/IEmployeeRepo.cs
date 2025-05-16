using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubidium
{
    public interface IEmployeeRepo : IRepository<Employee>
    {
        List<Employee> SearchByName(string name);
        List<Employee> GetByPosition(string position);
        void AssignToFlight(int employeeId, int flightId);
    }
}
