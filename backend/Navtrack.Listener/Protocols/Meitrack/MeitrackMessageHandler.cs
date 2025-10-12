using System;
using System.Collections.Generic;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Meitrack;

[Service(typeof(ICustomMessageHandler<MeitrackProtocol>))]
public class MeitrackMessageHandler : BaseMessageHandler<MeitrackProtocol>
{
    public override IEnumerable<DeviceMessageEntity>? ParseRange(MessageInput input)
    {
        IEnumerable<DeviceMessageEntity> locations = ParseRange(input, ParseText, ParseBinary_CCC, ParseBinary_CCE);

        return locations;
    }

    private static IEnumerable<DeviceMessageEntity>? ParseBinary_CCC(MessageInput input)
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

            List<DeviceMessageEntity> locations = [];

            while (input.DataMessage.ByteReader.BytesLeft > 52)
            {
                DeviceMessageEntity deviceMessage = new();

                byte eventCode = input.DataMessage.ByteReader.GetOne();

                deviceMessage.Latitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                deviceMessage.Longitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                deviceMessage.Date = new DateTime(2001, 1, 1)
                    .AddSeconds(input.DataMessage.ByteReader.Get<int>());
                deviceMessage.Valid = input.DataMessage.ByteReader.GetOne() == 0x01;
                deviceMessage.Satellites = input.DataMessage.ByteReader.GetOne();
                deviceMessage.GSMSignalStrength = input.DataMessage.ByteReader.GetOne();
                deviceMessage.Speed = input.DataMessage.ByteReader.Get<short>();
                deviceMessage.Heading = input.DataMessage.ByteReader.Get<short>();
                deviceMessage.HDOP = input.DataMessage.ByteReader.Get<short>() * 0.1f;
                deviceMessage.Altitude = input.DataMessage.ByteReader.Get<short>();
                deviceMessage.DeviceOdometer = input.DataMessage.ByteReader.Get<int>();
                int runTime = input.DataMessage.ByteReader.Get<int>();
                deviceMessage.GSMMobileCountryCode =
                    input.DataMessage.ByteReader.Get<short>().ToString();
                deviceMessage.GSMMobileNetworkCode =
                    input.DataMessage.ByteReader.Get<short>().ToString();
                deviceMessage.GSMLocationAreaCode =
                    input.DataMessage.ByteReader.Get<short>().ToString();
                deviceMessage.GSMCellId = input.DataMessage.ByteReader.Get<short>();

                input.DataMessage.ByteReader.Skip(12);

                locations.Add(deviceMessage);
            }

            return locations;
        }

        return null;
    }

    private IEnumerable<DeviceMessageEntity>? ParseBinary_CCE(MessageInput input)
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

            List<DeviceMessageEntity> deviceMessages = [];

            for (int i = 0; i < dataPackets; i++)
            {
                short dataPacketLength = input.DataMessage.ByteReader.Get<short>();
                short dataPacketId = input.DataMessage.ByteReader.Get<short>();

                DeviceMessageEntity deviceMessageDocument = new();

                int[] sizes = [1, 2, 4];

                foreach (int size in sizes)
                {
                    byte numberOfPackets = input.DataMessage.ByteReader.GetOne();

                    for (int j = 0; j < numberOfPackets; j++)
                    {
                        byte id = input.DataMessage.ByteReader.GetOne();

                        switch (id)
                        {
                            case 0x02:
                                deviceMessageDocument.Latitude =
                                    input.DataMessage.ByteReader.Get<int>() * 0.000001;
                                break;
                            case 0x03:
                                deviceMessageDocument.Longitude =
                                    input.DataMessage.ByteReader.Get<int>() * 0.000001;
                                break;
                            case 0x04:
                                deviceMessageDocument.Date = new DateTime(2001, 1, 1)
                                    .AddSeconds(input.DataMessage.ByteReader.Get<int>());
                                break;
                            case 0x05:
                                deviceMessageDocument.Valid =
                                    input.DataMessage.ByteReader.GetOne() == 0x01;
                                break;
                            case 0x06:
                                deviceMessageDocument.Satellites = input.DataMessage.ByteReader.GetOne();
                                break;
                            case 0x07:
                                deviceMessageDocument.GSMSignalStrength = input.DataMessage.ByteReader.GetOne();
                                break;
                            case 0x08:
                                deviceMessageDocument.Speed = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x09:
                                deviceMessageDocument.Heading = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x0A:
                                deviceMessageDocument.HDOP = input.DataMessage.ByteReader.Get<short>() * 0.1f;
                                break;
                            case 0x0B:
                                deviceMessageDocument.Altitude = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x0C:
                                deviceMessageDocument.DeviceOdometer = input.DataMessage.ByteReader.Get<int>();
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

                deviceMessages.Add(deviceMessageDocument);
            }

            return deviceMessages;
        }

        return null;
    }

    private static IEnumerable<DeviceMessageEntity>? ParseText(MessageInput input)
    {
        if (input.DataMessage.CommaSplit.Length > 14)
        {
            input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[1]);

            DeviceMessageEntity deviceMessage = new();

            deviceMessage.Latitude = input.DataMessage.CommaSplit.Get<double>(4);
            deviceMessage.Longitude = input.DataMessage.CommaSplit.Get<double>(5);
            deviceMessage.Date = ConvertDate(input.DataMessage.CommaSplit[6]);
            deviceMessage.Valid = input.DataMessage.CommaSplit.Get<string>(7) == "A";
            deviceMessage.Satellites = input.DataMessage.CommaSplit.Get<short>(8);
            deviceMessage.GSMSignalStrength = GsmUtil.ConvertSignal(input.DataMessage.CommaSplit.Get<short>(9));
            deviceMessage.Speed = input.DataMessage.CommaSplit.Get<short?>(10);
            deviceMessage.Heading = input.DataMessage.CommaSplit.Get<short?>(11);
            deviceMessage.HDOP = input.DataMessage.CommaSplit.Get<float?>(12);
            deviceMessage.Altitude = input.DataMessage.CommaSplit.Get<short>(13);
            deviceMessage.DeviceOdometer = input.DataMessage.CommaSplit.Get<int>(14);

            return [deviceMessage];
        }

        return null;
    }

    private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
        date[6..8], date[8..10], date[10..12]);
}