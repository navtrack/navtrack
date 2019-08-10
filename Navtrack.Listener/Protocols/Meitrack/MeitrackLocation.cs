using Navtrack.Common.Model;

namespace Navtrack.Listener.Protocols.Meitrack
{
    public class MeitrackLocation : Location {
        public string Message { get; set; }
        public int Odometer { get; set; }
    }
}