using System;
using System.Globalization;
using System.Linq;
using Navtrack.Common.Model;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Protocols.Meitrack
{
    [Service(typeof(IMeitrackLocationParser))]
    public class MeitrackLocationParser : IMeitrackLocationParser
    {
        public Location<MeitrackData> Parse(string input)
        {
            if (IsValidMessage(input))
            {
                string[] splitInput = input.Split(',');

                try
                {
                    Location<MeitrackData> location = new Location<MeitrackData>
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
                        Data = GetMeitrackData(input, splitInput)
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

        private static MeitrackData GetMeitrackData(string input, string[] splitInput)
        {
            string io = Utility.Hex2Bin(splitInput[17]);
            string[] baseId = splitInput[16].Split('|');
            string[] analogInput = splitInput[18].Split('|');

            MeitrackData meitrackData = new MeitrackData
            {
                GPSStatus = splitInput[7],
                Event = Enum.Parse<Event>(splitInput[3]),
                GSMSignal = Convert.ToInt32(splitInput[9]),
                Data = input,
                Journey = Convert.ToInt32(splitInput[14]),
                Runtime = Convert.ToInt32(splitInput[15]),
                Output = io.Take(8).Select(x => x == '1').ToArray(),
                Input = io.Skip(8).Take(8).Select(x => x == '1').ToArray(),
                MobileCountryCode = Convert.ToInt32(baseId[0]),
                MobileNetworkCode = Convert.ToInt32(baseId[1]),
                LocationAreaCode = int.Parse(baseId[2], NumberStyles.HexNumber),
                CellId = int.Parse(baseId[3], NumberStyles.HexNumber),
                Voltage = analogInput.Select(CalculateVoltage).ToArray()
            };

            return meitrackData;
        }

        private static long? CalculateVoltage(string input, int index)
        {
            Voltage voltage = (Voltage) index;

            if (int.TryParse(input, NumberStyles.HexNumber, null, out int value))
            {
                return voltage switch
                {
                    Voltage.AnalogInput1 => (value * 6 / 1024),
                    Voltage.AnalogInput2 => (value * 6 / 1024),
                    Voltage.AnalogInput3 => (value * 6 / 1024),
                    Voltage.Battery => (value * 3 * 2 / 1024),
                    Voltage.External => (value * 3 * 16 / 1024),
                    _ => 0
                };
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