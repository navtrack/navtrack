using System;

namespace Navtrack.Listener.Helpers;

public static class NumberExtensions
{ 
    public static short? ToShort(this float? x) {
        return x switch
        {
            null => null,
            < short.MinValue => short.MinValue,
            > short.MaxValue => short.MaxValue,
            _ => (short)Math.Round(x.Value)
        };
    }
    
    public static short? ToShort(this float x) {
        return x switch
        {
            < short.MinValue => short.MinValue,
            > short.MaxValue => short.MaxValue,
            _ => (short)Math.Round(x)
        };
    }
}