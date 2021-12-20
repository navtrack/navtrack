namespace Navtrack.Listener.Helpers;

public static class BitUtil
{
    public static bool IsTrue(int value, int index)
    {
        return (value & (1 << index)) != 0;
    }
        
    public static int ShiftRight(int value, int count) {
        return value >> count;
    }
}