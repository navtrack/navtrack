using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Listener.Services.Mappers;

public static class GsmElementMapper
{
    public static GsmElement? Map(GsmElement? destination)
    {
        if (destination != null)
        {
            destination.SignalLevel = destination.SignalLevel == 0 ? null : destination.SignalLevel;
        }

        return destination;
    }
}