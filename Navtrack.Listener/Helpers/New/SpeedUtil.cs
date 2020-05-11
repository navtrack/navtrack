namespace Navtrack.Listener.Helpers.New
{
    public abstract class SpeedUtil
    {
        public static decimal KnotsToKph(decimal knots)
        {
            return knots * (decimal) 1.852;
        }
    }
}