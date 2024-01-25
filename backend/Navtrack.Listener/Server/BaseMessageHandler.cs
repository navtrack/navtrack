using System;
using System.Collections.Generic;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public class BaseMessageHandler<T> : ICustomMessageHandler<T>
{
    public virtual Position Parse(MessageInput input)
    {
        return null;
    }

    public virtual IEnumerable<Position>? ParseRange(MessageInput input)
    {
        try
        {
            Position position = Parse(input);

            return position != null ? new[] {position} : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected Position Parse(MessageInput input, params Func<MessageInput, Position>[] parsers)
    {
        foreach (Func<MessageInput,Position> parse in parsers)
        {
            try
            {
                input.DataMessage.Reader.Reset();
                input.DataMessage.ByteReader.Reset();
                Position position = parse(input);

                if (position != null)
                {
                    return position;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return null;
    }
        
    protected IEnumerable<Position> ParseRange(MessageInput input, params Func<MessageInput, IEnumerable<Position>>[] parsers)
    {
        foreach (Func<MessageInput,IEnumerable<Position>> parse in parsers)
        {
            try
            {
                input.DataMessage.Reader.Reset();
                input.DataMessage.ByteReader.Reset();
                IEnumerable<Position> location = parse(input);

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