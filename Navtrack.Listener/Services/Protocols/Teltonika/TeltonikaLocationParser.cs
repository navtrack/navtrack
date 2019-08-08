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
        public List<LocationHolder> Convert(byte[] input, string imei)
        {
            List<LocationHolder> locations = new List<LocationHolder>();

            int? locationsIndex = GetLocationsReceivedIndex(input);

            if (locationsIndex.HasValue)
            {
                int locationsReceived = input[locationsIndex.Value];

                int lastIndex = 9;

                for (int i = 0; i < locationsReceived; i++)
                {
                    LocationHolder location = GetLocation(input, imei, ref lastIndex);

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

        private static LocationHolder GetLocation(byte[] input, string imei, ref int lastIndex)
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

            List<Event> events = new List<Event>();

            // 1 BYTE VALUE EVENTS
            events.AddRange(GetEvents(input, ref lastIndex, 1)); // skip 1 byte events

            // 2 BYTES VALUE EVENTS
            GetEvents(input, ref lastIndex, 2); // skip 2 byte events

            // 4 BYTES VALUE EVENTS
            GetEvents(input, ref lastIndex, 4); // skip 4 byte events

            // 8 BYTES VALUE EVENTS
            GetEvents(input, ref lastIndex, 8); // skip 8 byte events

            TeltonikaData teltonikaData = new TeltonikaData
            {
                Movement = GetMovement(events),
                Voltage = GetVoltage(events),
                Odometer = GetOdometer(events),
                CurrentProfile = GetCurrentProfile(events),
                GsmSignal = GetGsmSignal(events),
                MobileCountryCode = $"{(GetOperatorCode(events) / 100)}",
                MobileNetworkCode = $"{(GetOperatorCode(events) % 100)}",
                //DigitalInputs = GetDigitalInput(events),
                Events = events
            };

            location.ProtocolData = JsonSerializer.Serialize(teltonikaData);

            return new LocationHolder
            {
                Location = location,
                ProtocolData = teltonikaData
            };
        }

        private static int GetOperatorCode(IEnumerable<Event> events)
        {
            Event operatorCodeEvent = events.FirstOrDefault(x => x.Id == 241);

            return operatorCodeEvent?.Value ?? 0;
        }

        private static int GetGsmSignal(IEnumerable<Event> events)
        {
            Event gsmSignalEvent = events.FirstOrDefault(x => x.Id == 21);

            return gsmSignalEvent?.Value * 20 ?? 0;
        }

        private static string GetCurrentProfile(IEnumerable<Event> events)
        {
            Event currentProfileEvent = events.FirstOrDefault(x => x.Id == 22);

            return currentProfileEvent != null
                ? currentProfileEvent.Value.ToString(CultureInfo.InvariantCulture)
                : string.Empty;
        }

        private static bool GetMovement(IEnumerable<Event> events)
        {
            Event movementEvent = events.FirstOrDefault(x => x.Id == 240);

            return movementEvent?.Value == 1;
        }

        private static double GetVoltage(IEnumerable<Event> events)
        {
            Event voltageEvent = events.FirstOrDefault(x => x.Id == 66);

            return voltageEvent != null ? (double) voltageEvent.Value / 1000 : 0;
        }

        private static int GetOdometer(IEnumerable<Event> events)
        {
            Event odometerEvent = events.FirstOrDefault(x => x.Id == 199);

            return odometerEvent?.Value ?? 0;
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
                string eventId = Utility.ConvertToHex(input[position++]);
                string value = Utility.GetNextBytes(input, ref position, eventBytes);

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