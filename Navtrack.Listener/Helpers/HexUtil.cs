using System;
using System.Collections.Generic;
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
        
        public static byte[] ConvertHexStringArrayToByteArray(string[] hexArray)
        {
            return hexArray.Select(x => Convert.ToByte(x, 16)).ToArray();
        }
        
        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            return ConvertHexStringArrayToByteArray(ConvertHexToHexArray(hexString))
                .ToArray();
        }
        
        public static char[] ConvertHexToCharArray(string hexString)
        {
            return ConvertHexStringToByteArray(hexString).Select(Convert.ToChar).ToArray();
        }
        
        public static string ConvertHexStringToString(string hexString)
        {
            return new string(ConvertHexToCharArray(hexString));
        }
        
        public static string[] ConvertByteArrayToHexStringArray(IEnumerable<byte> byteArray)
        {
            return byteArray.Select(x => x.ToString("X2")).ToArray();
        }

        public static string[] ConvertIntArrayToHexStringArray(IEnumerable<int> byteArray)
        {
            return byteArray.Select(x => x.ToString("X2")).ToArray();
        }

        public static int[] ConvertHexStringToIntArray(string hex)
        {
            return ConvertHexStringArrayToIntArray(ConvertHexToHexArray(hex))
                .ToArray();
        }

        private static int[] ConvertHexStringArrayToIntArray(string[] hexArray)
        {
            return hexArray.Select(x => Convert.ToInt32(x, 16)).ToArray();
        }

        public static string ConvertHexStringArrayToHexString(string[] hex)
        {
            return String.Join("", hex);
        }
    }
}