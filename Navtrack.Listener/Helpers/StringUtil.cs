using System;
using System.Linq;
using static System.String;

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

        public static string Join(string[] array) => string.Join(Empty, array);

        public static string ConvertByteArrayToString(int[] bytes)
        {
            return new string(bytes.Select(x => (char)x).ToArray());
        }
    }
}