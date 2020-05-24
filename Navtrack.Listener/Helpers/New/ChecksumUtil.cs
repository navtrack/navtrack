namespace Navtrack.Listener.Helpers.New
{
    public abstract class ChecksumUtil
    {
        public static string NMEA(string input)
        {
            int checksum = 0;
            byte[] bytes = StringUtil.ConvertStringToByteArray(input);
            
            for (int i = 1; i < bytes.Length; i++)
            {
                checksum ^= bytes[i];
            }

            return $"*{checksum:X2}".ToUpper();
        }
    }
}