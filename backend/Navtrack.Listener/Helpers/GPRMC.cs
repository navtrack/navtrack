using System;
using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers.New;

namespace Navtrack.Listener.Helpers;

public class GPRMC
{
    public DateTime DateTime { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool PositionStatus { get; set; }
    public float? Speed { get; set; }
    public float? Heading { get; set; }

    // Input example: $GPRMC,102156.000,A,2232.4690,N,11403.6847,E,0.00,,180909,,*15
    public static GPRMC Parse(string input)
    {
        Match match = new Regex("(\\d{2}\\d{2}\\d{2}.\\d+)," + // dd mm yy
                                "(A|V)," + // gps fix
                                "(\\d+.\\d+),(N|S)," + // latitude
                                "(\\d+.\\d+),(E|W)," + // longitude
                                "(.*?)," + // speed
                                "(.*?)," + // heading
                                "(\\d{2}\\d{2}\\d{2})") // hh mm ss . ss
            .Match(input);

        if (match.Success)
        {
            GPRMC gprmc = new()
            {
                DateTime = NewDateTimeUtil.Convert(DateFormat.HHMMSS_SS_DDMMYY, match.Groups[1].Value,
                    match.Groups[9].Value),
                PositionStatus = match.Groups[2].Value == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(match.Groups[3].Value, match.Groups[4].Value),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(match.Groups[5].Value, match.Groups[6].Value),
                Speed = SpeedUtil.KnotsToKph(match.Groups[7].Get<float>()),
                Heading = match.Groups[8].Get<float?>()
            };

            return gprmc;
        }

        return null;
    }
}