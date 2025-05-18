using System.Collections.Generic;

namespace Rubidium
{
    public interface IAircraftRepo : IRepository<Aircraft>
    {
        Aircraft GetByRegistrationNumber(string registrationNumber);
        List<Aircraft> GetByModel(string model);
        List<Aircraft> GetByManufactureYear(int year);
    }
} 