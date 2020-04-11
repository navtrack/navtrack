using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Navtrack.Api.Services.LetsEncrypt
{
    internal class CertificateHostedService : IHostedService
    {
        private readonly ICertificateService certificateService;
        private readonly ILogger<CertificateHostedService> logger;

        public CertificateHostedService(ICertificateService certificateService, ILogger<CertificateHostedService> logger)
        {
            this.certificateService = certificateService;
            this.logger = logger;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await certificateService.Load(cancellationToken);
                }
                catch (Exception e)
                {
                    logger.Log(LogLevel.Error, e, "LetsEncrypt");
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }
    }
}