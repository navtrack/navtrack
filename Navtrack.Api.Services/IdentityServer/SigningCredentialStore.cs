using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.IdentityModel.Tokens;
using Navtrack.Api.Services.LetsEncrypt;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.IdentityServer
{
    [Service(typeof(ISigningCredentialStore))]
    public class SigningCredentialStore : ISigningCredentialStore
    {
        private readonly ICertificateProvider certificateProvider;

        public SigningCredentialStore(ICertificateProvider certificateProvider)
        {
            this.certificateProvider = certificateProvider;
        }

        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            X509Certificate2 certificate = await certificateProvider.GetCertificate();

            return certificate != null
                ? new SigningCredentials(new X509SecurityKey(certificate), SecurityAlgorithms.RsaSha256)
                : null;
        }
    }
}