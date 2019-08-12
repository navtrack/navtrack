using System;
using System.Globalization;
using System.Linq;
using static System.String;

namespace Navtrack.Listener.Protocols.Meitrack
{
    public static class Utility
    {
        public static DateTime ConvertDate(string date)
        {
            string year = date.Substring(0, 2);
            string month = date.Substring(2, 2);
            string day = date.Substring(4, 2);
            string hour = date.Substring(6, 2);
            string minute = date.Substring(8, 2);
            string second = date.Substring(10, 2);

            return new DateTime(Convert.ToInt32(year) + 2000,
                Convert.ToInt32(month),
                Convert.ToInt32(day),
                Convert.ToInt32(hour),
                Convert.ToInt32(minute),
                Convert.ToInt32(second));
        }

        public static float Hex2Volts(string input)
        {
            int number = int.Parse(input, NumberStyles.HexNumber);

            return (float) number / 1024 * 48;
        }

        public static string Hex2Bin(string input)
        {
            return Join(Empty,
                input.Select(x => Convert.ToString(Convert.ToInt32(x.ToString(), 16), 2).PadLeft(4, '0')));
        }
    }
}