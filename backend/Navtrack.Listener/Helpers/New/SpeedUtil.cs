namespace Navtrack.Listener.Helpers.New;

public abstract class SpeedUtil
{
    public static float? KnotsToKph(float knots)
    {
        return knots * 1.852f;
    }
}