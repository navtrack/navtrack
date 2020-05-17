using System.Threading.Tasks;
using Navtrack.DeviceData.Model;

namespace Navtrack.DeviceData.Services
{
    public interface IProtocolDataService
    {
        Protocol[] GetProtocols();
    }
}