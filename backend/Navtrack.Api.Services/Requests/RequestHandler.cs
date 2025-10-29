using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Requests;

[Service(typeof(IRequestHandler))]
public class RequestHandler(IServiceProvider serviceProvider, IPost post) : IRequestHandler
{
    public async Task Handle<TRequest>(TRequest request)
    {
        IRequestHandler<TRequest> handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();

        ValidationApiException validationException = new();

        await handler.Validate(new RequestValidationContext<TRequest>
        {
            Request = request,
            ValidationException = validationException
        });

        validationException.ThrowIfInvalid();

        await handler.Handle(request);

        IEvent? @event = handler.GetEvent(request);

        await SendEvent(@event);
    }

    public async Task<TResult> Handle<TRequest, TResult>(TRequest request)
    {
        IRequestHandler<TRequest, TResult> handler =
            serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();

        ValidationApiException validationException = new();

        await handler.Validate(new RequestValidationContext<TRequest>
        {
            Request = request,
            ValidationException = validationException
        });

        validationException.ThrowIfInvalid();

        TResult result = await handler.Handle(request);

        IEvent? @event = handler.GetEvent(request, result);

        await SendEvent(@event);

        return result;
    }

    private async Task SendEvent(IEvent? @event)
    {
        if (@event != null)
        {
            Type eventType = @event.GetType();

            MethodInfo? sendMethod = typeof(IPost).GetMethod(nameof(IPost.Send))?.MakeGenericMethod(eventType);

            if (sendMethod != null)
            {
                await (Task)sendMethod.Invoke(post, [@event]);
            }
        }
    }
}