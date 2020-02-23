using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;

namespace Navtrack.Web.Services.LetsEncrypt
{
    internal interface IServerCertificateSelector
    {
        void Add(X509Certificate2 certificate);
        X509Certificate2 Select(ConnectionContext features, string domainName);
    }
}