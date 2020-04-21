namespace Navtrack.Listener.Server
{
    public class MessageReader
    {
        private string message;

        public MessageReader(string message)
        {
            this.message = message;
        }

        public string Get(int i)
        {
            return GetNext(i, false);
        }

        private string GetNext(int i, bool skipOne)
        {
            string sub = message.Substring(0, i);

            message = message.Substring(skipOne ? i + 1 : i);
            
            return sub;
        } 

        public MessageReader Skip(int i)
        {
            message = message.Substring(i);

            return this;
        }

        public string GetUntil(char c)
        {
            int newIndex = message.IndexOf(c);
            
            return GetNext(newIndex, true);
        }
    }
}