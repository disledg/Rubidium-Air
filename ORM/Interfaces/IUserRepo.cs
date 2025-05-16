using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubidium
{
    public interface IUserRepo : IRepository<User>
    {
        User GetByUsername(string username);
        bool ValidateCredentials(string username, string password);
    }
}
