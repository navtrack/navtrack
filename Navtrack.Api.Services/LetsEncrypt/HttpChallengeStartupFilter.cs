using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Navtrack.Api.Services.LetsEncrypt
{
    internal class HttpChallengeStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Map("/.well-known/acme-challenge",
                    mapped =>
                    {
                        mapped.UseMiddleware<HttpChallengeMiddleware>();
                    });

                next(app);
            };
        }
    }
}