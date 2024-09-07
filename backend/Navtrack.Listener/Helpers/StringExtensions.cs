namespace Navtrack.Listener.Helpers;

public static class StringExtensions
{ 
    public static string ToHex(this string input)
    {
        return HexUtil.ConvertStringToHexString(input);
    }

    public static byte[] ToByteArray(this string input)
    {
        return StringUtil.ConvertStringToByteArray(input);
    }
}