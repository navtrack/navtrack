namespace Navtrack.Listener.Protocols.Meiligao
{
    public static class ArrayExtensions
    {
        public static string GetValueOrDefault(this string[] array, int index)
        {
            return array.Length > index ? array[index] : null;
        }
    }
}