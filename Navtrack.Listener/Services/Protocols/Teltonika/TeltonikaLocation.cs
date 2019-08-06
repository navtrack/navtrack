using System.Collections.Generic;
using Navtrack.Common.Model;

namespace Navtrack.Listener.Services.Protocols.Teltonika
{
    public class TeltonikaLocation : Location
    {
        public List<Event> Events { get; set; }
        public int Priority { get; set; }
        public int EventId { get; set; }
        public string DigitalInputs { get; set; }
        public bool Movement { get; set; }
        public int GsmSignal { get; set; }
        public string CurrentProfile { get; set; }
        public string MobileCountryCode { get; set; }
        public string MobileNetworkCode { get; set; }
    }
}