using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rubidium
{
    public class UserRepository : Repository<User>, IUserRepo
    {
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(AirportDBEntities1 context, IPasswordHasher passwordHasher)
            : base(context)
        {
            _passwordHasher = passwordHasher;
        }

        public User GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.Username == username);
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = GetByUsername(username);
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
                return false;

            return _passwordHasher.VerifyPassword(password, user.PasswordHash);
        }
    }
}
