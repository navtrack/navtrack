using System;
using System.Linq;
using System.Security.Cryptography;
using Navtrack.Library.DI;

namespace Navtrack.Common.Passwords;

[Service(typeof(IPasswordHasher))]
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 32;
    private const int KeySize = 64;
    private const int Iterations = 1000;
    private readonly HashAlgorithmName hashAlgorithmName  = HashAlgorithmName.SHA512;

    public (string, string) Hash(string password)
    {
        using Rfc2898DeriveBytes algorithm = new(
            password,
            SaltSize,
            Iterations,
            hashAlgorithmName);

        byte[] inArray = algorithm.GetBytes(KeySize);
        string hash = Convert.ToBase64String(inArray);
        string salt = Convert.ToBase64String(algorithm.Salt);

        return (hash, salt);
    }

    public bool CheckPassword(string password, string hash, string salt)
    {
        byte[] keyBytes = Convert.FromBase64String(hash);
        byte[] saltBytes = Convert.FromBase64String(salt);

        using Rfc2898DeriveBytes algorithm = new(
            password,
            saltBytes,
            Iterations,
            hashAlgorithmName);
            
        byte[] keyToCheck = algorithm.GetBytes(KeySize);

        bool verified = keyToCheck.SequenceEqual(keyBytes);

        return verified;
    }
}