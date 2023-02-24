using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navtrack.Api.Services.Environment;

public interface IEnvironmentService
{
    public Task<Dictionary<string, string>> Get();
}