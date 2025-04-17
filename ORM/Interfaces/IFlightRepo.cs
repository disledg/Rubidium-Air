using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubidium
{
    public interface IFlightRepo : IRepository<Flight>
    {
        Flight GetByNumber(string number);
        List<Flight> GetByDestination(string destination);
        List<Flight> GetByStatus(string status);
        List<Employee> GetCrew(int flightId);
    }
}
