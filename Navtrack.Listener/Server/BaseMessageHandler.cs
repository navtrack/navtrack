using System;
using System.Collections.Generic;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server
{
    public class BaseMessageHandler<T> : ICustomMessageHandler<T>
    {
        public virtual Location Parse(MessageInput input)
        {
            return null;
        }

        public virtual IEnumerable<Location> ParseRange(MessageInput input)
        {
            Location location = Parse(input);

            return location != null ? new[] {location} : null;
        }

        protected Location Parse(MessageInput input, params Func<MessageInput, Location>[] parsers)
        {
            foreach (Func<MessageInput,Location> parse in parsers)
            {
                try
                {
                    input.MessageData.Reader.Reset();
                    Location location = parse(input);

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
}