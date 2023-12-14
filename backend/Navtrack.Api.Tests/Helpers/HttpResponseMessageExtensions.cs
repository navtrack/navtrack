using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Navtrack.Api.Tests.Helpers;

public static class HttpResponseMessageExtensions
{
    public static async Task<T?> GetResult<T>(this HttpResponseMessage response)
    {
        string responseContent = await response.Content.ReadAsStringAsync();
        
        T? result = JsonConvert.DeserializeObject<T>(responseContent);

        return result;
    }
}