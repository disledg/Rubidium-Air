using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rubidium
{
    public class PasswordHasher : IPasswordHasher
    {
        // Настройки хеширования
        private const int SaltSize = 16; // 128 бит
        private const int HashSize = 32; // 256 бит
        private const int Iterations = 10000; // Рекомендуемое значение

        public string HashPassword(string password)
        {
            // Генерация соли
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Генерация хеша
            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password: password,
                salt: salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }

            // Сохраняем в формате "соль:хеш"
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            try
            {
                var parts = storedHash.Split(':');
                if (parts.Length != 2) return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] storedHashBytes = Convert.FromBase64String(parts[1]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password: password,
                    salt: salt,
                    iterations: Iterations,
                    hashAlgorithm: HashAlgorithmName.SHA256))
                {
                    byte[] computedHash = pbkdf2.GetBytes(HashSize);
                    return computedHash.SequenceEqual(storedHashBytes);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
