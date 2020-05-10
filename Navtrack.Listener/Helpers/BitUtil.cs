namespace Navtrack.Listener.Helpers
{
    public static class BitUtl
    {
        public static bool IsTrue(int value, int index)
        {
            return (value & (1 << index)) != 0;
        }
    }
}