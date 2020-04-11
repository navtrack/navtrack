using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.IdentityModel.Tokens;
using Navtrack.Api.Services.LetsEncrypt;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.IdentityServer
{
    [Service(typeof(IValidationKeysStore))]
    public class ValidationKeysStore : IValidationKeysStore
    {
        private readonly ICertificateProvider certificateProvider;

        public ValidationKeysStore(ICertificateProvider certificateProvider)
        {
            this.certificateProvider = certificateProvider;
        }

        public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
        {
            X509Certificate2 certificate = await certificateProvider.GetCertificate();

            return certificate != null
                ? new[]
                {
                    new SecurityKeyInfo
                    {
                        Key = new X509SecurityKey(certificate, SecurityAlgorithms.RsaSha256),
                        SigningAlgorithm = SecurityAlgorithms.RsaSha256
                    }
                }
                : Enumerable.Empty<SecurityKeyInfo>();
        }
    }
}