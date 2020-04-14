using System;
using System.Linq;

namespace Navtrack.Listener.Helpers
{
    public static class StringUtil
    { 
        public static byte[] ConvertStringToByteArray(string input)
        {
            return input.Select(Convert.ToByte).ToArray();
        }
        
        public static string[] ConvertStringToHexArray(string input)
        {
            return ConvertStringToByteArray(input)
                .Select(x => x.ToString("X2"))
                .ToArray();
        }

        public static string ConvertByteArrayToString(byte[] bytes)
        {
            return new string(bytes.Select(x => (char)x).ToArray());
        }
    }
}