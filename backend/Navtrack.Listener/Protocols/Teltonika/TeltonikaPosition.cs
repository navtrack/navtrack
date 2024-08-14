using System.Collections.Generic;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Protocols.Teltonika;

internal class TeltonikaPosition : Position
{
    public TeltonikaPosition()
    {
        PermanentElements = new TeltonikaPositionPermanentElements();
        OBDElements = new TeltonikaPositionOBDElements();
        OEMOBDElements = new TeltonikaPositionOEMOBDElements();
        DataPackets = new List<TeltonikaDataPacket>();
    }
    
    public TeltonikaPositionPermanentElements PermanentElements { get; set; }
    public TeltonikaPositionOBDElements OBDElements { get; set; }
    public TeltonikaPositionOEMOBDElements OEMOBDElements { get; set; }
    public List<TeltonikaDataPacket> DataPackets { get; set; }
}