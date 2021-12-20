using System;
using System.Linq;

namespace Navtrack.Listener.Helpers;

public static class StringUtil
{ 
    public static byte[] ConvertStringToByteArray(string input)
    {
        return input.Select(Convert.ToByte).ToArray();
    }
        
    public static string ConvertByteArrayToString(byte[] bytes)
    {
        return new string(bytes.Select(x => (char)x).ToArray());
    }
        
    public static bool IsDigitsOnly(string value)
    {
        return value.All(c => c >= '0' && c <= '9');
    }
}