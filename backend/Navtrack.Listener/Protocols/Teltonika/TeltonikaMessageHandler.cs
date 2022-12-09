using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Teltonika;

[Service(typeof(ICustomMessageHandler<TeltonikaProtocol>))]
public class TeltonikaMessageHandler : BaseMessageHandler<TeltonikaProtocol>
{
    public override IEnumerable<Location> ParseRange(MessageInput input)
    {
        if (input.Client.Device == null)
        {
            if (input.DataMessage.Bytes.Length > 16)
            {
                string imei = Encoding.ASCII.GetString(input.DataMessage.Bytes[2..17]);

                input.Client.SetDevice(imei);

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
        List<Location> locations = new();

        Codec codec = (Codec) input.DataMessage.ByteReader.Skip(8).GetOne();
        CodecConfiguration codecConfiguration = CodecConfiguration.Dictionary[codec];

        int noOfLocations = input.DataMessage.ByteReader.GetOne();

        for (int i = 0; i < noOfLocations; i++)
        {
            Location location = GetLocation(input, codec, codecConfiguration);

            locations.Add(location);
        }

        return locations;
    }

    private static Location GetLocation(MessageInput input, Codec codec, CodecConfiguration codecConfiguration)
    {
        Location location = new()
        {
            Device = input.Client.Device
        };

        location.DateTime = DateTime.UnixEpoch
            .AddMilliseconds(input.DataMessage.ByteReader.Get(8).ToInt64());

        byte priority = input.DataMessage.ByteReader.GetOne();

        location.Longitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        location.Latitude = GetCoordinate(input.DataMessage.ByteReader.Get(4));
        location.Altitude = input.DataMessage.ByteReader.Get(2).ToInt16();
        location.Heading = input.DataMessage.ByteReader.Get(2).ToInt16();
        location.Satellites = input.DataMessage.ByteReader.GetOne();
        location.Speed = input.DataMessage.ByteReader.Get(2).ToInt16();

        byte[] eventId = input.DataMessage.ByteReader.Get(codecConfiguration.MainEventIdLength);
        if (codecConfiguration.GenerationType)
        {
            byte generationType = input.DataMessage.ByteReader.GetOne();
        }

        byte[] totalEvents = input.DataMessage.ByteReader.Get(codecConfiguration.TotalEventsLength);

        List<Event> events = new();

        events.AddRange(GetEvents(input.DataMessage.ByteReader, 1, codecConfiguration)); // 1 byte events
        events.AddRange(GetEvents(input.DataMessage.ByteReader, 2, codecConfiguration)); // 2 bytes events
        events.AddRange(GetEvents(input.DataMessage.ByteReader, 4, codecConfiguration)); // 4 bytes events
        events.AddRange(GetEvents(input.DataMessage.ByteReader, 8, codecConfiguration)); // 8 bytes events
        events.AddRange(GetEvents(input.DataMessage.ByteReader, codecConfiguration)); // variable bytes events

        return location;
    }

    private static double GetCoordinate(byte[] coordinate)
    {
        double convertedCoordinate = coordinate.ToInt32();
        string binary = Convert.ToString(coordinate[0], 2).PadLeft(8, '0');
        bool isNegative = binary[0] == 1;

        if (isNegative)
        {
            convertedCoordinate *= -1;
        }

        return convertedCoordinate / 10000000;
    }

    private static IEnumerable<Event> GetEvents(ByteReader input, int eventBytes,
        CodecConfiguration codecConfiguration)
    {
        List<Event> events = new();

        short numberOfEvents = codecConfiguration.EventsLength == 1
            ? input.GetOne()
            : input.Get(2).ToInt16();

        for (int i = 0; i < numberOfEvents; i++)
        {
            events.Add(new Event
            {
                Id = codecConfiguration.EventIdLength == 1
                    ? input.GetOne()
                    : input.Get(2).ToInt16(),
                Value = input.Get(eventBytes)
            });
        }

        return events;
    }

    private static IEnumerable<Event> GetEvents(ByteReader input, CodecConfiguration codecConfiguration)
    {
        List<Event> events = new();

        if (codecConfiguration.HasVariableSizeElements)
        {
            short numberOfEvents = input.Get(2).ToInt16();

            for (int i = 0; i < numberOfEvents; i++)
            {
                events.Add(new Event
                {
                    Id = input.Get(2).ToInt16(),
                    Value = input.Get(input.Get(2).ToInt16())
                });
            }
        }

        return events;
    }
}