using System;
using System.Globalization;
using System.Linq;
using Navtrack.Common.Model;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Services.Protocols.Meitrack
{
    [Service(typeof(IMeitrackLocationParser))]
    public class MeitrackLocationParser : IMeitrackLocationParser
    {
        public MeitrackLocation Parse(string input)
        {
            if (IsValidMessage(input))
            {
                string[] splitInput = input.Split(',');

                try
                {
                    MeitrackLocation location = new MeitrackLocation
                    {
                        Device = new Device
                        {
                            IMEI = splitInput[1]
                            //Voltage = Utility.Hex2Volts(splitInput[4]) TODO
                        },
                        Latitude = Convert.ToDecimal(splitInput[4], CultureInfo.InvariantCulture),
                        Longitude = Convert.ToDecimal(splitInput[5], CultureInfo.InvariantCulture),
                        DateTime = Utility.ConvertDate(splitInput[6]),
                        Satellites = Convert.ToInt16(splitInput[8]),
                        Speed = Convert.ToInt32(splitInput[10]),
                        Heading = Convert.ToInt32(splitInput[11]),
                        HDOP = Convert.ToDouble(splitInput[12]),
                        Altitude = Convert.ToInt32(splitInput[13]),
                        Odometer = Convert.ToInt32(splitInput[14]),
                        Message = input
                    };

                    return location;
                }
                catch (IndexOutOfRangeException)
                {
                    // ignore
                }
            }

            return null;
        }

        private static bool IsValidMessage(string input) =>
            !string.IsNullOrEmpty(input) && CalculateChecksum(input) == GetChecksum(input);

        private static string CalculateChecksum(string input)
        {
            int checksumPosition = GetChecksumPosition(input);

            int sum = input
                .Substring(0, checksumPosition)
                .Aggregate(0, (i, c) => i + (int) c);


            string hexSum = sum.ToString("X");

            string checksum = hexSum.Substring(Math.Max(0, hexSum.Length - 2));

            return checksum;
        }

        private static string GetChecksum(string input)
        {
            int checksumPosition = GetChecksumPosition(input);

            if (checksumPosition != 0)
            {
                string checksum = input.Substring(checksumPosition, input.Length - checksumPosition);

                return checksum;
            }

            return string.Empty;
        }

        private static int GetChecksumPosition(string input) =>
            input.LastIndexOf("*", StringComparison.InvariantCultureIgnoreCase) + 1;
    }
}