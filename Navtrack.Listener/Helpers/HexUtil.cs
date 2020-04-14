using System;
using System.Linq;

namespace Navtrack.Listener.Helpers
{
    public static class HexUtil
    { 
        public static string[] ConvertHexToHexArray(string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => hexString.Substring(x, 2))
                .ToArray();
        }
        
        public static byte[] ConvertHexArrayToByteArray(string[] hexArray)
        {
            return hexArray.Select(x => Convert.ToByte(x, 16)).ToArray();
        }
        
        public static byte[] ConvertHexToByteArray(string hexString)
        {
            return ConvertHexArrayToByteArray(ConvertHexToHexArray(hexString))
                .ToArray();
        }
        
        public static char[] ConvertHexToCharArray(string hexString)
        {
            return ConvertHexToByteArray(hexString).Select(Convert.ToChar).ToArray();
        }
        
        public static string ConvertHexStringToString(string hexString)
        {
            return new string(ConvertHexToCharArray(hexString));
        }
        
        public static string[] ConvertByteArrayToHexArray(byte[] byteArray)
        {
            return byteArray.Select(x => x.ToString("X2")).ToArray();
        }
    }
}