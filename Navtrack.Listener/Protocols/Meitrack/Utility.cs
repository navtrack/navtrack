using System;
using System.Globalization;

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

        public static string Hex2Bin(char input)
        {
            switch (input)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'A':
                    return "1010";
                case 'B':
                    return "1011";
                case 'C':
                    return "1100";
                case 'D':
                    return "1101";
                case 'E':
                    return "1110";
                case 'F':
                    return "1111";
            }

            return string.Empty;
        }
    }
}