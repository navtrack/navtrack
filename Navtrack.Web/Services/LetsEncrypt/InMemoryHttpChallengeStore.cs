using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Library.DI;

namespace Navtrack.Web.Services.LetsEncrypt
{
    [Service(typeof(IHttpChallengeStore), ServiceLifetime.Singleton)]
    internal class InMemoryHttpChallengeStore : IHttpChallengeStore
    {
        private readonly ConcurrentDictionary<string, string> values
            = new ConcurrentDictionary<string, string>();

        public void AddChallengeResponse(string token, string response)
            => values.AddOrUpdate(token, response, (_, __) => response);

        public bool TryGetResponse(string token, [MaybeNullWhen(false)] out string value)
            => values.TryGetValue(token, out value);
    }
}