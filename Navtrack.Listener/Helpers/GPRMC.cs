using System;
using System.Text.RegularExpressions;

namespace Navtrack.Listener.Helpers
{
    // $GPRMC,102156.000,A,2232.4690,N,11403.6847,E,0.00,,180909,,*15
    public class GPRMC
    {
        public DateTime DateTime { get; }
        public decimal Latitude { get; }
        public decimal Longitude { get; }
        public bool PositionStatus { get; }
        public double Speed { get; }
        public float? Heading { get; }

        public GPRMC(string input)
        {
            input = input.Replace("$GPRMC,", String.Empty);
            string[] split = input.Split(",");

            DateTime = GetDateTime(split[0], split[8]);
            Latitude = GpsUtil.ConvertDmmToDecimal(@"(\d{2})(\d{2}.\d{4})", split[2], split[3]);
            Longitude = GpsUtil.ConvertDmmToDecimal(@"(\d{3})(\d{2}.\d{4})", split[4], split[5]);
            PositionStatus = split[1] == "A";
            Speed = split.Get<double>(6) * 1.852;
            Heading = split.Get<float?>(7);
        }

        private static DateTime GetDateTime(string timeString, string dateString)
        {
            GroupCollection time = new Regex(@"(\d{2})(\d{2})(\d{2}).(\d{2})").Matches(timeString)[0].Groups;

            GroupCollection date = new Regex(@"(\d{2})(\d{2})(\d{2})").Matches(dateString)[0].Groups;
            return DateTimeUtil.New(date[3].Value, date[2].Value, date[1].Value,
                time[1].Value, time[2].Value, time[3].Value,
                time[4].Value);
        }
    }
}