using System;
using System.Collections.Generic;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Listener.Server;

public class BaseMessageHandler<T> : ICustomMessageHandler<T>
{
    public virtual DeviceMessageDocument Parse(MessageInput input)
    {
        return null;
    }

    public virtual IEnumerable<DeviceMessageDocument>? ParseRange(MessageInput input)
    {
        try
        {
            DeviceMessageDocument deviceMessageDocument = Parse(input);

            return deviceMessageDocument != null ? new[] {deviceMessageDocument} : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected DeviceMessageDocument Parse(MessageInput input, params Func<MessageInput, DeviceMessageDocument>[] parsers)
    {
        foreach (Func<MessageInput,DeviceMessageDocument> parse in parsers)
        {
            try
            {
                input.DataMessage.Reader.Reset();
                input.DataMessage.ByteReader.Reset();
                DeviceMessageDocument deviceMessageDocument = parse(input);

                if (deviceMessageDocument != null)
                {
                    return deviceMessageDocument;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return null;
    }
        
    protected IEnumerable<DeviceMessageDocument> ParseRange(MessageInput input, params Func<MessageInput, IEnumerable<DeviceMessageDocument>>[] parsers)
    {
        foreach (Func<MessageInput,IEnumerable<DeviceMessageDocument>> parse in parsers)
        {
            try
            {
                input.DataMessage.Reader.Reset();
                input.DataMessage.ByteReader.Reset();
                IEnumerable<DeviceMessageDocument> location = parse(input);

                if (location != null)
                {
                    return location;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return null;
    }
}