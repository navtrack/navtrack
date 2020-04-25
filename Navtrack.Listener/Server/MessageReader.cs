namespace Navtrack.Listener.Server
{
    public class MessageReader
    {
        private string message;
        private int index;

        public MessageReader(string message)
        {
            this.message = message;
            index = 0;
        }

        public string Get(int i)
        {
            return GetNext(i, false);
        }

        private string GetNext(int i, bool skipOne)
        {
            string sub = message.Substring(index, i);

            index = skipOne ? index + i + 1 : index + i;

            return sub;
        } 

        public MessageReader Skip(int i)
        {
            // message = message.Substring(i);
            index += i;

            return this;
        }

        public string GetUntil(char c)
        {
            int newIndex = message.Substring(index).IndexOf(c);
            
            return GetNext(newIndex, true);
        }

        public void Reset()
        {
            index = 0;
        }
    }
}