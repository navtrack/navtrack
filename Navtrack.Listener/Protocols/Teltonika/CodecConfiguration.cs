using System.Collections.Generic;

namespace Navtrack.Listener.Protocols.Teltonika;

public class CodecConfiguration
{
    public readonly int MainEventIdLength;
    public readonly int TotalEventsLength;
    public readonly bool GenerationType;
    public readonly int EventsLength;
    public readonly int EventIdLength;
    public readonly bool HasVariableSizeElements;

    private CodecConfiguration(int mainEventIdLength, int totalEventsLength, bool generationType, int eventsLength,
        int eventIdLength, bool hasVariableSizeElements)
    {
        MainEventIdLength = mainEventIdLength;
        TotalEventsLength = totalEventsLength;
        GenerationType = generationType;
        EventsLength = eventsLength;
        EventIdLength = eventIdLength;
        HasVariableSizeElements = hasVariableSizeElements;
    }

    public static readonly Dictionary<Codec, CodecConfiguration> Dictionary =
        new()
        {
            {Codec.Codec8, new CodecConfiguration(1, 1, false, 1, 1, false)},
            {Codec.Codec8Extended, new CodecConfiguration(2, 2, false, 2, 2, true)},
            {Codec.Codec16, new CodecConfiguration(2, 1, true, 1, 2, false)}
        };
}