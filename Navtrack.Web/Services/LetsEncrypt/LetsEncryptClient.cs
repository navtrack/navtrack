using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Certes;
using Certes.Acme;
using Certes.Acme.Resource;
using Certes.Pkcs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Navtrack.Common.Services;
using Navtrack.Library.DI;

namespace Navtrack.Web.Services.LetsEncrypt
{
    [Service(typeof(ILetsEncryptClient), ServiceLifetime.Singleton)]
    internal class LetsEncryptClient : ILetsEncryptClient
    {
        private readonly IDbConfiguration dbConfiguration;
        private readonly ILogger<LetsEncryptClient> logger;
        private readonly IHttpChallengeStore httpChallengeStore;
        private AcmeContext AcmeContext { get; }
        private string Email { get; set; }
        private string Hostname { get; set; }

        public LetsEncryptClient(IDbConfiguration dbConfiguration, ILogger<LetsEncryptClient> logger,
            IHttpChallengeStore httpChallengeStore, IHostEnvironment hostEnvironment)
        {
            this.dbConfiguration = dbConfiguration;
            this.logger = logger;
            this.httpChallengeStore = httpChallengeStore;
            
            AcmeContext = new AcmeContext(hostEnvironment.IsDevelopment()
                ? WellKnownServers.LetsEncryptStagingV2
                : WellKnownServers.LetsEncryptV2);
        }

        public async Task<X509Certificate2> GetCertificate()
        {
            Email = await dbConfiguration.Get<WebConfiguration>(x => x.EmailAddress);
            Hostname = await dbConfiguration.Get<WebConfiguration>(x => x.Hostname);

            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Hostname))
            {
                X509Certificate2 certificate = await RequestNewCertificate();
                
                return certificate;
            }

            logger.LogDebug("LetsEncrypt: email or hostname not configured.");

            return null;
        }

        private async Task<X509Certificate2> RequestNewCertificate()
        {
            await AcmeContext.NewAccount(Email, termsOfServiceAgreed: true);
            
            IOrderContext order = await AcmeContext.NewOrder(new List<string> {Hostname});

            IEnumerable<IAuthorizationContext> authorizations = await order.Authorizations();

            await Task.WhenAll(authorizations.Select(Validate));
      
            X509Certificate2 certificate = await CreateCertificate(order);

            return certificate;
        }

        private async Task Validate(IAuthorizationContext authorizationContext)
        {
            IChallengeContext httpChallenge = await authorizationContext.Http();
            
            httpChallengeStore.AddChallengeResponse(httpChallenge.Token, httpChallenge.KeyAuthz);

            await httpChallenge.Validate();

            for (int i = 0; i < 10; i++)
            {
                Authorization authorization = await authorizationContext.Resource();

                if (authorization.Status is AuthorizationStatus.Valid)
                {
                    return;
                }
                
                await Task.Delay(TimeSpan.FromMilliseconds(2));
            }
        }

        private async Task<X509Certificate2> CreateCertificate(IOrderContext order)
        {
            IKey privateKey = KeyFactory.NewKey(KeyAlgorithm.ES256);

            CertificateChain certificateChain = await order.Generate(new CsrInfo
            {
                CommonName = Hostname
            }, privateKey);

            PfxBuilder pfxBuilder = certificateChain.ToPfx(privateKey);
            byte[] pfx = pfxBuilder.Build(Hostname, string.Empty);
            
            return new X509Certificate2(pfx, string.Empty, X509KeyStorageFlags.Exportable);
        }
    }
}