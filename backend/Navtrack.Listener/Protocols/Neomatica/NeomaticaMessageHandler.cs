using System;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Neomatica;

[Service(typeof(ICustomMessageHandler<NeomaticaProtocol>))]
public class NeomaticaMessageHandler : BaseMessageHandler<NeomaticaProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        input.DataMessage.ByteReader.Get<short>(); // device id

        int size = input.DataMessage.ByteReader.GetOne();
        int type = input.DataMessage.ByteReader.GetOne();

        if (type == 0x03) // imei
        {
            input.ConnectionContext.SetDevice(StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.Get(15)));
        }
        else
        {
            return GetLocation(input, type);
        }

        return null;
    }


    private static DeviceMessageDocument GetLocation(MessageInput input, int type)
    {
        if (input.ConnectionContext.Device == null)
        {
            return null;
        }

        DeviceMessageDocument deviceMessageDocument = new()
        {
            // Device = input.ConnectionContext.Device,
            Position = new PositionElement()
        };

        input.DataMessage.ByteReader.GetOne();
        input.DataMessage.ByteReader.Get<short>();

        short status = input.DataMessage.ByteReader.Get<short>();
        deviceMessageDocument.Position = new PositionElement();
        deviceMessageDocument.Position.Valid = !BitUtil.IsTrue(status, 5);
        deviceMessageDocument.Position.Latitude = input.DataMessage.ByteReader.Get<float>();
        deviceMessageDocument.Position.Longitude = input.DataMessage.ByteReader.Get<float>();
        deviceMessageDocument.Position.Heading = input.DataMessage.ByteReader.Get<short>() * 0.1f;
        deviceMessageDocument.Position.Speed = input.DataMessage.ByteReader.Get<short>() * 0.1f;
        input.DataMessage.ByteReader.GetOne();
        deviceMessageDocument.Position.Altitude = input.DataMessage.ByteReader.Get<short>();
        deviceMessageDocument.Position.HDOP = input.DataMessage.ByteReader.GetOne() * 0.1f;
        deviceMessageDocument.Position.Satellites = (short?)(input.DataMessage.ByteReader.GetOne() & 0x0f);
        deviceMessageDocument.Position.Date = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());

        MarkAsNull(deviceMessageDocument);

        return deviceMessageDocument;
    }

    private static void MarkAsNull(DeviceMessageDocument deviceMessageDocument)
    {
        if (!deviceMessageDocument.Position.Valid.GetValueOrDefault())
        {
            deviceMessageDocument.Position.Heading = null;
            deviceMessageDocument.Position.Speed = null;
            deviceMessageDocument.Position.Altitude = null;
            deviceMessageDocument.Position.HDOP = null;
            deviceMessageDocument.Position.Satellites = null;
        }
    }
}