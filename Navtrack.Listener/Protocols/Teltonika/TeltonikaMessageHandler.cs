using System;
using System.Collections;
using System.Collections.Generic;
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

            List<Location> locations = GetLocations(input);

            if (locations.Any())
            {
                string reply = locations.Count.ToString("X8");

                input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
            }

            return locations;
        }

        private static List<Location> GetLocations(MessageInput input)
        {
            List<Location> locations = new List<Location>();

            Codec codec = (Codec) input.MessageData.ByteReader.Skip(8).GetOne();
            CodecConfiguration codecConfiguration = CodecConfiguration.Dictionary[codec];

            int noOfLocations = input.MessageData.ByteReader.GetOne();

            for (int i = 0; i < noOfLocations; i++)
            {
                Location location = GetLocation(input, codec, codecConfiguration);

                locations.Add(location);
            }

            return locations;
        }

        private static Location GetLocation(MessageInput input, Codec codec, CodecConfiguration codecConfiguration)
        {
            Location location = new Location
            {
                Device = input.Client.Device
            };

            location.DateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0)
                .AddMilliseconds(BitConverter.ToInt64(input.MessageData.ByteReader.Get(8).Reverse().ToArray()));

            byte priority = input.MessageData.ByteReader.GetOne();

            location.Longitude = GetCoordinate(input.MessageData.ByteReader.Get(4));
            location.Latitude = GetCoordinate(input.MessageData.ByteReader.Get(4));
            location.Altitude = BitConverter.ToInt16(input.MessageData.ByteReader.Get(2).Reverse().ToArray());
            location.Heading = BitConverter.ToInt16(input.MessageData.ByteReader.Get(2).Reverse().ToArray());
            location.Satellites = input.MessageData.ByteReader.GetOne();
            location.Speed = BitConverter.ToInt16(input.MessageData.ByteReader.Get(2).Reverse().ToArray());

            byte[] eventId = input.MessageData.ByteReader.Get(codecConfiguration.MainEventIdLength);
            if (codecConfiguration.GenerationType)
            {
                byte generationType = input.MessageData.ByteReader.GetOne();
            }

            byte[] totalEvents = input.MessageData.ByteReader.Get(codecConfiguration.TotalEventsLength);

            List<Event> events = new List<Event>();

            events.AddRange(GetEvents(input.MessageData.ByteReader, 1, codecConfiguration)); // 1 byte events
            events.AddRange(GetEvents(input.MessageData.ByteReader, 2, codecConfiguration)); // 2 bytes events
            events.AddRange(GetEvents(input.MessageData.ByteReader, 4, codecConfiguration)); // 4 bytes events
            events.AddRange(GetEvents(input.MessageData.ByteReader, 8, codecConfiguration)); // 8 bytes events
            events.AddRange(GetEvents(input.MessageData.ByteReader, codecConfiguration)); // variable bytes events

            return location;
        }

        private static decimal GetCoordinate(byte[] coordinate)
        {
            decimal convertedCoordinate = BitConverter.ToInt32(coordinate);
            BitArray binaryCoordinate = new BitArray(coordinate);

            bool isNegative = binaryCoordinate[0];

            if (isNegative)
            {
                convertedCoordinate *= -1;
            }

            return convertedCoordinate / 10000000;
        }

        private static IEnumerable<Event> GetEvents(ByteReader input, int eventBytes,
            CodecConfiguration codecConfiguration)
        {
            List<Event> events = new List<Event>();

            short numberOfEvents = codecConfiguration.EventsLength == 1
                ? input.GetOne()
                : BitConverter.ToInt16(input.Get(2).Reverse().ToArray());

            for (int i = 0; i < numberOfEvents; i++)
            {
                events.Add(new Event
                {
                    Id = codecConfiguration.EventIdLength == 1
                        ? input.GetOne()
                        : BitConverter.ToInt16(input.Get(2).Reverse().ToArray()),
                    Value = input.Get(eventBytes)
                });
            }

            return events;
        }
        
        private static IEnumerable<Event> GetEvents(ByteReader input, CodecConfiguration codecConfiguration)
        {
            List<Event> events = new List<Event>();

            if (codecConfiguration.HasVariableSizeElements)
            {
                short numberOfEvents = BitConverter.ToInt16(input.Get(2).Reverse().ToArray());
                
                for (int i = 0; i < numberOfEvents; i++)
                {
                    events.Add(new Event
                    {
                        Id = BitConverter.ToInt16(input.Get(2).Reverse().ToArray()),
                        Value = input.Get(BitConverter.ToInt16(input.Get(2).Reverse().ToArray()))
                    });
                }
            }
            
            return events;
        }
    }
}