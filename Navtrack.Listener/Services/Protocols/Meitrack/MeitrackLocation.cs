using Navtrack.Common.Model;

namespace Navtrack.Listener.Services.Protocols.Meitrack
{
    public class MeitrackLocation : Location {
        public string Message { get; set; }
        public int Odometer { get; set; }
    }
}