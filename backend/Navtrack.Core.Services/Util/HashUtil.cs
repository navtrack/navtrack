using System;
using System.Security.Cryptography;
using System.Text;

namespace Navtrack.Core.Services.Util;

public static class HashUtil
{
    public static string GenerateSha256Hash(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = SHA256.HashData(bytes);
        string hash = Convert.ToBase64String(hashBytes);
        
        return hash;
    }
}