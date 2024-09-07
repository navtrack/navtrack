namespace Navtrack.Listener.Helpers;

public abstract class SpeedUtil
{
    public static float? KnotsToKph(float knots)
    {
        return knots * 1.852f;
    }
}