namespace Navtrack.Listener.Helpers;

public abstract class SpeedUtil
{
    public static short? KnotsToKph(float knots)
    {
        return (knots * 1.852f).ToShort();
    }
}