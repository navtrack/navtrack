using System.Collections.Generic;

namespace Navtrack.Listener.Protocols.Teltonika;

internal class TeltonikaCodecConfiguration
{
    public readonly int MainEventIdLength;
    public readonly int DataPacketCountBytes;
    public readonly bool HasGenerationType;
    public readonly int DataPacketBytes;
    public readonly int DataPacketIdBytes;
    public readonly bool HasVariableDataPackets;

    private TeltonikaCodecConfiguration(int mainEventIdLength, bool hasGenerationType,
        int dataPacketCountBytes,
        int dataPacketBytes, int dataPacketIdBytes, bool hasVariableDataPackets)
    {
        MainEventIdLength = mainEventIdLength;
        DataPacketCountBytes = dataPacketCountBytes;
        HasGenerationType = hasGenerationType;
        DataPacketBytes = dataPacketBytes;
        DataPacketIdBytes = dataPacketIdBytes;
        HasVariableDataPackets = hasVariableDataPackets;
    }

    public static readonly Dictionary<TeltonikaCodec, TeltonikaCodecConfiguration?> GetAll =
        new()
        {
            { TeltonikaCodec.Codec8, new TeltonikaCodecConfiguration(1, false, 1, 1, 1, false) },
            { TeltonikaCodec.Codec8Extended, new TeltonikaCodecConfiguration(2, false, 2, 2, 2, true) },
            { TeltonikaCodec.Codec16, new TeltonikaCodecConfiguration(2, true, 1, 1, 2, false) }
        };
}