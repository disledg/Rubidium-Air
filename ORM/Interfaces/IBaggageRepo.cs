using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubidium
{
    public interface IBaggageRepo : IRepository<Baggage>
    {
        void UpdateStatus(int id, string newStatus);
        List<Baggage> GetByFlightId(int flightId);
        List<Baggage> GetByPassengerNumber(int passengerNumber);
    }
}
