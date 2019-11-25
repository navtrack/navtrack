using System.Threading.Tasks;

namespace Navtrack.Listener.Services.Logging
{
    public interface ILogger
    {
        Task Log(string data);
    }
}