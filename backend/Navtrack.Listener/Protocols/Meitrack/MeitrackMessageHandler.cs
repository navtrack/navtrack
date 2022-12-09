using System;
using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Meitrack;

[Service(typeof(ICustomMessageHandler<MeitrackProtocol>))]
public class MeitrackMessageHandler : BaseMessageHandler<MeitrackProtocol>
{
    public override IEnumerable<Location> ParseRange(MessageInput input)
    {
        IEnumerable<Location> locations = ParseRange(input, ParseText, ParseBinary_CCC, ParseBinary_CCE);

        return locations;
    }

    private static IEnumerable<Location> ParseBinary_CCC(MessageInput input)
    {
        input.DataMessage.ByteReader.Skip(2); // header
        input.DataMessage.ByteReader.Skip(1); // data identifier
        input.DataMessage.ByteReader.Skip(3); // data length
        input.DataMessage.ByteReader.Skip(1); // ,

        string imei = StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.GetUntil(0x2C));
        string command = StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.GetUntil(0x2C));

        input.Client.SetDevice(imei);

        if (command == "CCC")
        {
            short protocolVersion = input.DataMessage.ByteReader.Get<short>();
            short dataPackets = input.DataMessage.ByteReader.Get<short>();
            int cacheRecords = input.DataMessage.ByteReader.Get<int>();

            List<Location> locations = new();

            while (input.DataMessage.ByteReader.BytesLeft > 52)
            {
                Location location = new()
                {
                    Device = input.Client.Device
                };

                byte eventCode = input.DataMessage.ByteReader.GetOne();

                location.Latitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                location.Longitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                location.DateTime = new DateTime(2001, 1, 1)
                    .AddSeconds(input.DataMessage.ByteReader.Get<int>());
                location.PositionStatus = input.DataMessage.ByteReader.GetOne() == 0x01;
                location.Satellites = input.DataMessage.ByteReader.GetOne();
                location.GsmSignal = input.DataMessage.ByteReader.GetOne();
                location.Speed = input.DataMessage.ByteReader.Get<short>();
                location.Heading = input.DataMessage.ByteReader.Get<short>();
                location.HDOP = input.DataMessage.ByteReader.Get<short>() * 0.1f;
                location.Altitude = input.DataMessage.ByteReader.Get<short>();
                location.Odometer = input.DataMessage.ByteReader.Get<int>();
                int runTime = input.DataMessage.ByteReader.Get<int>();
                location.MobileCountryCode = input.DataMessage.ByteReader.Get<short>();
                location.MobileNetworkCode = input.DataMessage.ByteReader.Get<short>();
                location.LocationAreaCode = input.DataMessage.ByteReader.Get<short>();
                location.CellId = input.DataMessage.ByteReader.Get<short>();

                input.DataMessage.ByteReader.Skip(12);

                locations.Add(location);
            }

            return locations;
        }

        return null;
    }

    private IEnumerable<Location> ParseBinary_CCE(MessageInput input)
    {
        input.DataMessage.ByteReader.Skip(2); // header
        input.DataMessage.ByteReader.Skip(1); // data identifier
        input.DataMessage.ByteReader.Skip(3); // data length
        input.DataMessage.ByteReader.Skip(1); // ,

        string imei = StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.GetUntil(0x2C));
        string command = StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.GetUntil(0x2C));

        input.Client.SetDevice(imei);

        if (command == "CCE")
        {
            int cacheRecords = input.DataMessage.ByteReader.Get<int>();
            short dataPackets = input.DataMessage.ByteReader.Get<short>();

            List<Location> locations = new();

            for (int i = 0; i < dataPackets; i++)
            {
                short dataPacketLength = input.DataMessage.ByteReader.Get<short>();
                short dataPacketId = input.DataMessage.ByteReader.Get<short>();

                Location location = new()
                {
                    Device = input.Client.Device
                };

                int[] sizes = {1, 2, 4};

                foreach (int size in sizes)
                {
                    byte numberOfPackets = input.DataMessage.ByteReader.GetOne();

                    for (int j = 0; j < numberOfPackets; j++)
                    {
                        byte id = input.DataMessage.ByteReader.GetOne();

                        switch (id)
                        {
                            case 0x02:
                                location.Latitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                                break;
                            case 0x03:
                                location.Longitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                                break;
                            case 0x04:
                                location.DateTime = new DateTime(2001, 1, 1)
                                    .AddSeconds(input.DataMessage.ByteReader.Get<int>());
                                break;
                            case 0x05:
                                location.PositionStatus = input.DataMessage.ByteReader.GetOne() == 0x01;
                                break;
                            case 0x06:
                                location.Satellites = input.DataMessage.ByteReader.GetOne();
                                break;
                            case 0x07:
                                location.GsmSignal = input.DataMessage.ByteReader.GetOne();
                                break;
                            case 0x08:
                                location.Speed = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x09:
                                location.Heading = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x0A:
                                location.HDOP = input.DataMessage.ByteReader.Get<short>() * 0.1f;
                                break;
                            case 0x0B:
                                location.Altitude = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x0C:
                                location.Odometer = input.DataMessage.ByteReader.Get<int>();
                                break;
                            default:
                                byte[] value = input.DataMessage.ByteReader.Get(size);
                                break;
                        }
                    }
                }

                byte numberOfNBytePackets = input.DataMessage.ByteReader.GetOne();
                for (int j = 0; j < numberOfNBytePackets; j++)
                {
                    byte id = input.DataMessage.ByteReader.GetOne();
                    byte size = input.DataMessage.ByteReader.GetOne();
                    input.DataMessage.ByteReader.Skip(size);
                }

                locations.Add(location);
            }

            return locations;
        }

        return null;
    }

    private static IEnumerable<Location> ParseText(MessageInput input)
    {
        if (input.DataMessage.CommaSplit.Length > 14)
        {
            input.Client.SetDevice(input.DataMessage.CommaSplit[1]);

            Location location = new()
            {
                Device = input.Client.Device,
                Latitude = input.DataMessage.CommaSplit.Get<double>(4),
                Longitude = input.DataMessage.CommaSplit.Get<double>(5),
                DateTime = ConvertDate(input.DataMessage.CommaSplit[6]),
                PositionStatus = input.DataMessage.CommaSplit.Get<string>(7) == "A",
                Satellites = input.DataMessage.CommaSplit.Get<short>(8),
                GsmSignal = GsmUtil.ConvertSignal(input.DataMessage.CommaSplit.Get<short>(9)),
                Speed = input.DataMessage.CommaSplit.Get<float?>(10),
                Heading = input.DataMessage.CommaSplit.Get<float?>(11),
                HDOP = input.DataMessage.CommaSplit.Get<float?>(12),
                Altitude = input.DataMessage.CommaSplit.Get<int>(13),
                Odometer = input.DataMessage.CommaSplit.Get<uint>(14)
            };

            return new[] {location};
        }

        return null;
    }

    private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
        date[6..8], date[8..10], date[10..12]);
}