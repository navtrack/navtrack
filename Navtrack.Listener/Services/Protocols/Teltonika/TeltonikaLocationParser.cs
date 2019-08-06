using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Navtrack.Common.Model;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Services.Protocols.Teltonika
{
    [Service(typeof(ITeltonikaLocationParser))]
    public class TeltonikaLocationParser : ITeltonikaLocationParser
    {
        public IEnumerable<Location> Convert(byte[] input, string imei)
        {
            List<Location> locations = new List<Location>();

            int? locationsIndex = GetLocationsReceivedIndex(input);

            if (locationsIndex.HasValue)
            {
                int locationsReceived = input[locationsIndex.Value];

                int lastIndex = 9;

                for (int i = 0; i < locationsReceived; i++)
                {
                    Location location = GetLocation(input, imei, ref lastIndex);

                    locations.Add(location);
                }
            }
            
            return locations;
        }

        private static int? GetLocationsReceivedIndex(byte[] input)
        {
            for (int index = 0; index + 8 < input.Length; index++)
            {
                if (input[index] == 0 &&
                    input[index + 1] == 0 &&
                    input[index + 2] == 0 &&
                    input[index + 3] == 0 &&
                    input[index + 7] == 8) return index + 8;
            }

            return null;
        }

        private static Location GetLocation(byte[] input, string imei, ref int lastIndex)
        {
            Location location = new Location
            {
                Device = new Device
                {
                    IMEI = imei
                }
            };

            int startIndex = lastIndex;

            // TIME
            string tmp = Utility.GetNextBytes(input, ref lastIndex, 8);
            location.DateTime = Utility.UnixTimeStampToDateTime(long.Parse(tmp, NumberStyles.HexNumber));

            // PRIORITY
            lastIndex++; // skip priority

            // LONGITUDE
            tmp = Utility.GetNextBytes(input, ref lastIndex, 4);
            location.Longitude = GetCoordinate(tmp);

            // LATITUDE
            tmp = Utility.GetNextBytes(input, ref lastIndex, 4);
            location.Latitude = GetCoordinate(tmp);

            // ALTITUDE
            tmp = Utility.GetNextBytes(input, ref lastIndex, 2);
            location.Altitude = int.Parse(tmp, NumberStyles.HexNumber);

            // ANGLE
            tmp = Utility.GetNextBytes(input, ref lastIndex, 2);
            location.Heading = int.Parse(tmp, NumberStyles.HexNumber);

            // SATELLITES
            tmp = Utility.ConvertToHex(input[lastIndex++]);
            location.Satellites = short.Parse(tmp, NumberStyles.HexNumber);

            // SPEED
            tmp = Utility.GetNextBytes(input, ref lastIndex, 2);
            location.Speed = int.Parse(tmp, NumberStyles.HexNumber);

            // EVENT ID
            lastIndex++; // skip event id

            // NUMBER of events
            lastIndex++; // skip over the number of events

            // 1 BYTE VALUE EVENTS
            GetEvents(input, ref lastIndex, 1); // skip 1 byte events

            // 2 BYTES VALUE EVENTS
            GetEvents(input, ref lastIndex, 2); // skip 2 byte events

            // 4 BYTES VALUE EVENTS
            GetEvents(input, ref lastIndex, 4); // skip 4 byte events

            // 8 BYTES VALUE EVENTS
            GetEvents(input, ref lastIndex, 8); // skip 8 byte events

            location.ProtocolData = JsonSerializer.Serialize((input.Skip(startIndex).Take(lastIndex - startIndex)));

            return location;
        }

        private static decimal GetCoordinate(string coordinate)
        {
            decimal convertedCoordinate = long.Parse(coordinate, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            string binaryCoordinate = Utility.HexToBinary(coordinate);

            bool isNegative = binaryCoordinate[0] == 1;

            if (isNegative)
            {
                convertedCoordinate *= -1;
            }

            return convertedCoordinate / 10000000;
        }

        private static IEnumerable<Event> GetEvents(byte[] input, ref int position, int eventBytes)
        {
            List<Event> events = new List<Event>();

            int numberOfEvents = input[position++];

            for (int i = 0; i < numberOfEvents; i++)
            {
                var eventId = Utility.ConvertToHex(input[position++]);
                var value = Utility.GetNextBytes(input, ref position, eventBytes);

                events.Add(new Event
                {
                    Id = int.Parse(eventId, NumberStyles.HexNumber),
                    Value = int.Parse(value, NumberStyles.HexNumber)
                });
            }

            return events;
        }
    }
}