using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Requests;

[Service(typeof(IRequestHandler))]
public class RequestHandler(IServiceProvider serviceProvider) : IRequestHandler
{
    public async Task Handle<TRequest>(TRequest request)
    {
        IRequestHandler<TRequest> handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();

        await handler.Handle(request);
    }

    public async Task<TResult> Handle<TRequest, TResult>(TRequest request)
    {
        IRequestHandler<TRequest, TResult> handler =
            serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();

        TResult result = await handler.Handle(request);

        return result;
    }
}
