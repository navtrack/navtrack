using System.Threading.Tasks;

namespace Navtrack.Common.Services.Logging
{
    public interface ILogger
    {
        Task Log(string data);
    }
}