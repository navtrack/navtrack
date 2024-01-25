using System;
using System.Collections.Generic;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Meitrack;

[Service(typeof(ICustomMessageHandler<MeitrackProtocol>))]
public class MeitrackMessageHandler : BaseMessageHandler<MeitrackProtocol>
{
    public override IEnumerable<Position>? ParseRange(MessageInput input)
    {
        IEnumerable<Position> locations = ParseRange(input, ParseText, ParseBinary_CCC, ParseBinary_CCE);

        return locations;
    }

    private static IEnumerable<Position> ParseBinary_CCC(MessageInput input)
    {
        input.DataMessage.ByteReader.Skip(2); // header
        input.DataMessage.ByteReader.Skip(1); // data identifier
        input.DataMessage.ByteReader.Skip(3); // data length
        input.DataMessage.ByteReader.Skip(1); // ,

        string imei = StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.GetUntil(0x2C));
        string command = StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.GetUntil(0x2C));

        input.ConnectionContext.SetDevice(imei);

        if (command == "CCC")
        {
            short protocolVersion = input.DataMessage.ByteReader.Get<short>();
            short dataPackets = input.DataMessage.ByteReader.Get<short>();
            int cacheRecords = input.DataMessage.ByteReader.Get<int>();

            List<Position> locations = [];

            while (input.DataMessage.ByteReader.BytesLeft > 52)
            {
                Position position = new()
                {
                    Device = input.ConnectionContext.Device
                };

                byte eventCode = input.DataMessage.ByteReader.GetOne();

                position.Latitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                position.Longitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                position.Date = new DateTime(2001, 1, 1)
                    .AddSeconds(input.DataMessage.ByteReader.Get<int>());
                position.PositionStatus = input.DataMessage.ByteReader.GetOne() == 0x01;
                position.Satellites = input.DataMessage.ByteReader.GetOne();
                position.GsmSignal = input.DataMessage.ByteReader.GetOne();
                position.Speed = input.DataMessage.ByteReader.Get<short>();
                position.Heading = input.DataMessage.ByteReader.Get<short>();
                position.HDOP = input.DataMessage.ByteReader.Get<short>() * 0.1f;
                position.Altitude = input.DataMessage.ByteReader.Get<short>();
                position.Odometer = input.DataMessage.ByteReader.Get<int>();
                int runTime = input.DataMessage.ByteReader.Get<int>();
                position.MobileCountryCode = input.DataMessage.ByteReader.Get<short>();
                position.MobileNetworkCode = input.DataMessage.ByteReader.Get<short>();
                position.LocationAreaCode = input.DataMessage.ByteReader.Get<short>();
                position.CellId = input.DataMessage.ByteReader.Get<short>();

                input.DataMessage.ByteReader.Skip(12);

                locations.Add(position);
            }

            return locations;
        }

        return null;
    }

    private IEnumerable<Position> ParseBinary_CCE(MessageInput input)
    {
        input.DataMessage.ByteReader.Skip(2); // header
        input.DataMessage.ByteReader.Skip(1); // data identifier
        input.DataMessage.ByteReader.Skip(3); // data length
        input.DataMessage.ByteReader.Skip(1); // ,

        string imei = StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.GetUntil(0x2C));
        string command = StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.GetUntil(0x2C));

        input.ConnectionContext.SetDevice(imei);

        if (command == "CCE")
        {
            int cacheRecords = input.DataMessage.ByteReader.Get<int>();
            short dataPackets = input.DataMessage.ByteReader.Get<short>();

            List<Position> locations = [];

            for (int i = 0; i < dataPackets; i++)
            {
                short dataPacketLength = input.DataMessage.ByteReader.Get<short>();
                short dataPacketId = input.DataMessage.ByteReader.Get<short>();

                Position position = new()
                {
                    Device = input.ConnectionContext.Device
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
                                position.Latitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                                break;
                            case 0x03:
                                position.Longitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                                break;
                            case 0x04:
                                position.Date = new DateTime(2001, 1, 1)
                                    .AddSeconds(input.DataMessage.ByteReader.Get<int>());
                                break;
                            case 0x05:
                                position.PositionStatus = input.DataMessage.ByteReader.GetOne() == 0x01;
                                break;
                            case 0x06:
                                position.Satellites = input.DataMessage.ByteReader.GetOne();
                                break;
                            case 0x07:
                                position.GsmSignal = input.DataMessage.ByteReader.GetOne();
                                break;
                            case 0x08:
                                position.Speed = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x09:
                                position.Heading = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x0A:
                                position.HDOP = input.DataMessage.ByteReader.Get<short>() * 0.1f;
                                break;
                            case 0x0B:
                                position.Altitude = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x0C:
                                position.Odometer = input.DataMessage.ByteReader.Get<int>();
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

                locations.Add(position);
            }

            return locations;
        }

        return null;
    }

    private static IEnumerable<Position> ParseText(MessageInput input)
    {
        if (input.DataMessage.CommaSplit.Length > 14)
        {
            input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[1]);

            Position position = new()
            {
                Device = input.ConnectionContext.Device,
                Latitude = input.DataMessage.CommaSplit.Get<double>(4),
                Longitude = input.DataMessage.CommaSplit.Get<double>(5),
                Date = ConvertDate(input.DataMessage.CommaSplit[6]),
                PositionStatus = input.DataMessage.CommaSplit.Get<string>(7) == "A",
                Satellites = input.DataMessage.CommaSplit.Get<short>(8),
                GsmSignal = GsmUtil.ConvertSignal(input.DataMessage.CommaSplit.Get<short>(9)),
                Speed = input.DataMessage.CommaSplit.Get<float?>(10),
                Heading = input.DataMessage.CommaSplit.Get<float?>(11),
                HDOP = input.DataMessage.CommaSplit.Get<float?>(12),
                Altitude = input.DataMessage.CommaSplit.Get<int>(13),
                Odometer = input.DataMessage.CommaSplit.Get<uint>(14)
            };

            return new[] {position};
        }

        return null;
    }

    private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
        date[6..8], date[8..10], date[10..12]);
}