using System;
using System.Text.RegularExpressions;

// ReSharper disable IdentifierTypo
namespace Navtrack.Listener.Helpers.New
{
    public abstract class NewDateTimeUtil
    {
        public static DateTime Convert(DateFormat dateFormat, params string[] input)
        {
            return Parse_HHMMSS_SS_DDMMYY(input);
        }
 
        private static DateTime Parse_HHMMSS_SS_DDMMYY(string[] input)
        {
            Match timeMatch = new Regex("(\\d{2})(\\d{2})(\\d{2}).(\\d+)").Match(input[0]); // hh mm ss . sss
            Match dateMatch = new Regex("(\\d{2})(\\d{2})(\\d{2})").Match(input[1]); // dd mm yy

            return DateTimeUtil.New(
                dateMatch.Groups[3].Value,
                dateMatch.Groups[2].Value,
                dateMatch.Groups[1].Value,
                timeMatch.Groups[1].Value,
                timeMatch.Groups[2].Value,
                timeMatch.Groups[3].Value,
                timeMatch.Groups[4].Value);
        }
    }
}