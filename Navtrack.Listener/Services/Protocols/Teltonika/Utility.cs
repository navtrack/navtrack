using System;
using System.Linq;

namespace Navtrack.Listener.Services.Protocols.Teltonika
{
    public static class Utility
    {
        public static string GetNextBytes(byte[] input, ref int position, int noOfBytes)
        {
            string tmp = string.Empty;

            for (int i = 0; i < noOfBytes; i++)
            {
                tmp += ConvertToHex(input[position++]);
            }

            return tmp;
        }

        public static string ConvertToHex(int input)
        {
            string hexValue = input.ToString("X");

            if (hexValue.Length == 1) return "0" + hexValue;

            return hexValue;
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string HexToBinary(string hexValue)
        {
            return Convert.ToString(Convert.ToInt32(hexValue, 16), 2);
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            dateTime = dateTime.AddMilliseconds(unixTimeStamp);

            return dateTime;
        }
    }
}