using System.Threading.Tasks;

namespace Navtrack.Common.Services
{
    public interface ILogger
    {
        Task Log(string data);
    }
}