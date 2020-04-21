using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Teltonika
{
    [Service(typeof(ICustomMessageHandler<TeltonikaProtocol>))]
    public class TeltonikaMessageHandler : BaseMessageHandler<TeltonikaProtocol>
    {
        public override IEnumerable<Location> ParseRange(MessageInput input)
        {
            if (input.Client.Device == null)
            {
                if (input.MessageData.Bytes.Length > 16)
                {
                    string imei = Encoding.ASCII.GetString(input.MessageData.Bytes[2..17]);

                    input.Client.Device = new Device
                    {
                        IMEI = imei
                    };

                    input.NetworkStream.WriteByte(1);
                }

                return null;
            }

            List<Location> locations = new List<Location>();

            int? locationsIndex = GetLocationsReceivedIndex(input.MessageData.Bytes);

            try
            {
                if (locationsIndex.HasValue)
                {
                    int locationsReceived = input.MessageData.Bytes[locationsIndex.Value];

                    int lastIndex = locationsIndex.Value + 1;

                    for (int i = 0; i < locationsReceived; i++)
                    {
                        Location location = GetLocation(input.MessageData.Bytes, input.Client.Device.IMEI,
                            ref lastIndex);

                        locations.Add(location);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (locations.Any())
            {
                string reply = locations.Count.ToString("X8");

                input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
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

            // TIME
            string tmp = TeltonikaUtil.GetNextBytes(input, ref lastIndex, 8);
            location.DateTime = TeltonikaUtil.UnixTimeStampToDateTime(long.Parse(tmp, NumberStyles.HexNumber));

            // PRIORITY
            lastIndex++; // skip priority

            // LONGITUDE
            tmp = TeltonikaUtil.GetNextBytes(input, ref lastIndex, 4);
            location.Longitude = GetCoordinate(tmp);

            // LATITUDE
            tmp = TeltonikaUtil.GetNextBytes(input, ref lastIndex, 4);
            location.Latitude = GetCoordinate(tmp);

            // ALTITUDE
            tmp = TeltonikaUtil.GetNextBytes(input, ref lastIndex, 2);
            location.Altitude = int.Parse(tmp, NumberStyles.HexNumber);

            // ANGLE
            tmp = TeltonikaUtil.GetNextBytes(input, ref lastIndex, 2);
            location.Heading = int.Parse(tmp, NumberStyles.HexNumber);

            // SATELLITES
            tmp = input[lastIndex++].ToString("X2");
            location.Satellites = short.Parse(tmp, NumberStyles.HexNumber);

            // SPEED
            tmp = TeltonikaUtil.GetNextBytes(input, ref lastIndex, 2);
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

            return location;
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
            string binaryCoordinate = Convert.ToString(Convert.ToInt32(coordinate, 16), 2);

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
                string eventId = input[position++].ToString("X2");
                string value = TeltonikaUtil.GetNextBytes(input, ref position, eventBytes);

                events.Add(new Event
                {
                    Id = int.Parse(eventId, NumberStyles.HexNumber),
                    Value = int.Parse(value, NumberStyles.HexNumber)
                });
            }

            return events;
        }

        public string GetIMEI(byte[] bytes)
        {
            string imei = String.Empty;

            for (int i = 1; i <= 15; i++)
            {
                imei += (char) bytes[i];
            }

            return imei;
        }
    }
}