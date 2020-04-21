using System;

namespace Navtrack.Listener.Protocols.Teltonika
{
    public static class TeltonikaUtil
    {
        public static string GetNextBytes(byte[] input, ref int position, int noOfBytes)
        {
            string tmp = string.Empty;

            for (int i = 0; i < noOfBytes; i++)
            {
                tmp += input[position++].ToString("X2");
            }

            return tmp;
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            dateTime = dateTime.AddMilliseconds(unixTimeStamp);

            return dateTime;
        }
    }
}