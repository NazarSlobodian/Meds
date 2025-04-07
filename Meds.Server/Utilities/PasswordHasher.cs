using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Meds.Utilities
{
    public class PasswordHasher
    {
        private const int saltSize = 16;
        private const int keySize = 32;
        private const int iterations = 100_000;

        public static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[saltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(keySize);

            var hashBytes = new byte[saltSize + keySize];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, saltSize);
            Buffer.BlockCopy(key, 0, hashBytes, saltSize, keySize);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            
            byte[] salt = new byte[saltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, saltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] keyToCheck = pbkdf2.GetBytes(keySize);

            for (int i = 0; i < keySize; i++)
            {
                if (hashBytes[saltSize + i] != keyToCheck[i])
                    return false;
            }
            return true;
        }
    }
}
