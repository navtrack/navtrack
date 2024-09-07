using System;
using System.Collections.Generic;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Meitrack;

[Service(typeof(ICustomMessageHandler<MeitrackProtocol>))]
public class MeitrackMessageHandler : BaseMessageHandler<MeitrackProtocol>
{
    public override IEnumerable<DeviceMessageDocument>? ParseRange(MessageInput input)
    {
        IEnumerable<DeviceMessageDocument> locations = ParseRange(input, ParseText, ParseBinary_CCC, ParseBinary_CCE);

        return locations;
    }

    private static IEnumerable<DeviceMessageDocument> ParseBinary_CCC(MessageInput input)
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

            List<DeviceMessageDocument> locations = [];

            while (input.DataMessage.ByteReader.BytesLeft > 52)
            {
                DeviceMessageDocument deviceMessageDocument = new()
                {
                    Position = new PositionElement(),
                    Gsm = new GsmElement()
                };

                byte eventCode = input.DataMessage.ByteReader.GetOne();

                deviceMessageDocument.Position.Latitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                deviceMessageDocument.Position.Longitude = input.DataMessage.ByteReader.Get<int>() * 0.000001;
                deviceMessageDocument.Position.Date = new DateTime(2001, 1, 1)
                    .AddSeconds(input.DataMessage.ByteReader.Get<int>());
                deviceMessageDocument.Position.Valid = input.DataMessage.ByteReader.GetOne() == 0x01;
                deviceMessageDocument.Position.Satellites = input.DataMessage.ByteReader.GetOne();
                deviceMessageDocument.Gsm.SignalStrength = input.DataMessage.ByteReader.GetOne();
                deviceMessageDocument.Position.Speed = input.DataMessage.ByteReader.Get<short>();
                deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.Get<short>();
                deviceMessageDocument.Position.HDOP = input.DataMessage.ByteReader.Get<short>() * 0.1f;
                deviceMessageDocument.Position.Altitude = input.DataMessage.ByteReader.Get<short>();
                deviceMessageDocument.Device ??= new DeviceElement();
                deviceMessageDocument.Device.Odometer = input.DataMessage.ByteReader.Get<int>();
                int runTime = input.DataMessage.ByteReader.Get<int>();
                deviceMessageDocument.Gsm.MobileCountryCode =
                    input.DataMessage.ByteReader.Get<short>().ToString();
                deviceMessageDocument.Gsm.MobileNetworkCode =
                    input.DataMessage.ByteReader.Get<short>().ToString();
                deviceMessageDocument.Gsm.LocationAreaCode =
                    input.DataMessage.ByteReader.Get<short>().ToString();
                deviceMessageDocument.Gsm.CellId = input.DataMessage.ByteReader.Get<short>();

                input.DataMessage.ByteReader.Skip(12);

                locations.Add(deviceMessageDocument);
            }

            return locations;
        }

        return null;
    }

    private IEnumerable<DeviceMessageDocument> ParseBinary_CCE(MessageInput input)
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

            List<DeviceMessageDocument> locations = [];

            for (int i = 0; i < dataPackets; i++)
            {
                short dataPacketLength = input.DataMessage.ByteReader.Get<short>();
                short dataPacketId = input.DataMessage.ByteReader.Get<short>();

                DeviceMessageDocument deviceMessageDocument = new()
                {
                    Position = new PositionElement(),
                    Gsm = new GsmElement()
                };

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
                                deviceMessageDocument.Position.Latitude =
                                    input.DataMessage.ByteReader.Get<int>() * 0.000001;
                                break;
                            case 0x03:
                                deviceMessageDocument.Position.Longitude =
                                    input.DataMessage.ByteReader.Get<int>() * 0.000001;
                                break;
                            case 0x04:
                                deviceMessageDocument.Position.Date = new DateTime(2001, 1, 1)
                                    .AddSeconds(input.DataMessage.ByteReader.Get<int>());
                                break;
                            case 0x05:
                                deviceMessageDocument.Position.Valid =
                                    input.DataMessage.ByteReader.GetOne() == 0x01;
                                break;
                            case 0x06:
                                deviceMessageDocument.Position.Satellites = input.DataMessage.ByteReader.GetOne();
                                break;
                            case 0x07:
                                deviceMessageDocument.Gsm.SignalStrength = input.DataMessage.ByteReader.GetOne();
                                break;
                            case 0x08:
                                deviceMessageDocument.Position.Speed = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x09:
                                deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x0A:
                                deviceMessageDocument.Position.HDOP = input.DataMessage.ByteReader.Get<short>() * 0.1f;
                                break;
                            case 0x0B:
                                deviceMessageDocument.Position.Altitude = input.DataMessage.ByteReader.Get<short>();
                                break;
                            case 0x0C:
                                deviceMessageDocument.Device ??= new DeviceElement();
                                deviceMessageDocument.Device.Odometer = input.DataMessage.ByteReader.Get<int>();
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

                locations.Add(deviceMessageDocument);
            }

            return locations;
        }

        return null;
    }

    private static IEnumerable<DeviceMessageDocument> ParseText(MessageInput input)
    {
        if (input.DataMessage.CommaSplit.Length > 14)
        {
            input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[1]);

            DeviceMessageDocument deviceMessageDocument = new()
            {
                Position = new PositionElement(),
                Gsm = new GsmElement()
            };

            // deviceMessageDocument. Device = input.ConnectionContext.Device,
            deviceMessageDocument.Position.Latitude = input.DataMessage.CommaSplit.Get<double>(4);
            deviceMessageDocument.Position.Longitude = input.DataMessage.CommaSplit.Get<double>(5);
            deviceMessageDocument.Position.Date = ConvertDate(input.DataMessage.CommaSplit[6]);
            deviceMessageDocument.Position.Valid = input.DataMessage.CommaSplit.Get<string>(7) == "A";
            deviceMessageDocument.Position.Satellites = input.DataMessage.CommaSplit.Get<short>(8);
            deviceMessageDocument.Gsm.SignalStrength =
                GsmUtil.ConvertSignal(input.DataMessage.CommaSplit.Get<short>(9));
            deviceMessageDocument.Position.Speed = input.DataMessage.CommaSplit.Get<float?>(10);
            deviceMessageDocument.Position.Heading = input.DataMessage.CommaSplit.Get<float?>(11);
            deviceMessageDocument.Position.HDOP = input.DataMessage.CommaSplit.Get<double?>(12);
            deviceMessageDocument.Position.Altitude = input.DataMessage.CommaSplit.Get<int>(13);
            deviceMessageDocument.Device ??= new DeviceElement();
            deviceMessageDocument.Device.Odometer = input.DataMessage.CommaSplit.Get<int>(14);

            return [deviceMessageDocument];
        }

        return null;
    }

    private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
        date[6..8], date[8..10], date[10..12]);
}