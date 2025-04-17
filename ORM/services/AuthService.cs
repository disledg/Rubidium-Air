using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rubidium
{
    public class AuthService
    {
        private readonly IUserRepo _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepo userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public bool Register(string username, string password)
        {
            if (_userRepository.GetByUsername(username) != null)
                return false;

            var passwordHash = _passwordHasher.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash
            };

            _userRepository.Add(user);
            _userRepository.Save();
            return true;
        }

        public User Login(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null) return null;

            return _userRepository.ValidateCredentials(username, password) ? user : null;
        }
    }
}
