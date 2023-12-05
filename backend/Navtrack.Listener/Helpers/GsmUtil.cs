namespace Navtrack.Listener.Helpers;

public static class GsmUtil
{
    public static short? ConvertSignal(short? signal)
    {
        return signal.HasValue ? (short) (signal.Value * 100 / 31) : null;
    }
}