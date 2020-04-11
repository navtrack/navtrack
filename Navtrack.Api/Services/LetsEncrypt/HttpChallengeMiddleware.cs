using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.LetsEncrypt
{
    [Service(typeof(HttpChallengeMiddleware))]
    internal class HttpChallengeMiddleware : IMiddleware
    {
        private readonly IHttpChallengeStore httpChallengeStore;

        public HttpChallengeMiddleware(IHttpChallengeStore httpChallengeStore)
        {
            this.httpChallengeStore = httpChallengeStore;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string token = context.Request.Path.ToString();
            
            if (token.StartsWith("/"))
            {
                token = token.Substring(1);
            }

            if (!httpChallengeStore.TryGetResponse(token, out string value))
            {
                await next(context);
                
                return;
            }

            context.Response.ContentLength = value?.Length ?? 0;
            context.Response.ContentType = "application/octet-stream";
            
            await context.Response.WriteAsync(value, context.RequestAborted);
        }
    }
}