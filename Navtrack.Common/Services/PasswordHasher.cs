using System;
using System.Linq;
using System.Security.Cryptography;
using Navtrack.Library.DI;

namespace Navtrack.Common.Services
{
    [Service(typeof(IPasswordHasher))]
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 32;
        private const int KeySize = 64;
        private const int Iterations = 1000;
        private readonly HashAlgorithmName hashAlgorithmName  = HashAlgorithmName.SHA512;

        public (string, string) Hash(string password)
        {
            using Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                hashAlgorithmName);

            byte[] inArray = algorithm.GetBytes(KeySize);
            string key = Convert.ToBase64String(inArray);
            string salt = Convert.ToBase64String(algorithm.Salt);

            return (salt, key);
        }

        public bool CheckPassword(string password, string salt, string hash)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] keyBytes = Convert.FromBase64String(hash);

            using Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(
                password,
                saltBytes,
                Iterations,
                hashAlgorithmName);
            
            byte[] keyToCheck = algorithm.GetBytes(KeySize);

            bool verified = keyToCheck.SequenceEqual(keyBytes);

            return verified;
        }
    }
}