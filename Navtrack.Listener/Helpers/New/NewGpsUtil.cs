using System.Collections.Generic;
using System.Text.RegularExpressions;

// ReSharper disable IdentifierTypo
namespace Navtrack.Listener.Helpers.New;

public abstract class NewGpsUtil
{
    public static double Convert(GpsFormat dateFormat, params string[] input)
    {
        return dateFormat switch
        {
            _ => Parse_DDDmmmmmm(input)
        };
    }

    private static double Parse_DDDmmmmmm(IReadOnlyList<string> input)
    {
        Match coordinateMatch = new Regex("(\\d{2,3})(\\d{2})(\\d{4})").Match(input[0]); // DD(D) mm mmmm

        return GpsUtil.ConvertDmmLatToDecimal(
            $"{coordinateMatch.Groups[1].Value}{coordinateMatch.Groups[2].Value}.{coordinateMatch.Groups[3].Value}",
            input[1]);
    }
}