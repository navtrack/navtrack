using System;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.LetsEncrypt
{
    [Service(typeof(IServerCertificateSelector), ServiceLifetime.Singleton)]
    internal sealed class ServerCertificateSelector : IServerCertificateSelector
    {
        private readonly ConcurrentDictionary<string, X509Certificate2> certificates =
            new ConcurrentDictionary<string, X509Certificate2>(StringComparer.OrdinalIgnoreCase);

        public void Add(X509Certificate2 certificate)
        {
            string hostName = certificate.GetNameInfo(X509NameType.SimpleName, false);

            certificates.AddOrUpdate(hostName, certificate, (x, y) => certificate);
        }

        public X509Certificate2 Select(ConnectionContext features, string hostName)
        {
            if (!string.IsNullOrEmpty(hostName))
            {
                certificates.TryGetValue(hostName, out X509Certificate2 certificate);

                return certificate;
            }

            return null;
        }
    }
}