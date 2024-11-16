using System;
using System.Threading.Tasks;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Requests;

public abstract class BaseRequestHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult>
{
    public virtual Task Validate(RequestValidationContext<TRequest> context)
    {
        return Task.CompletedTask;
    }

    public virtual Task<TResult> Handle(TRequest request)
    {
        throw new NotImplementedException();
    }

    public virtual IEvent? GetEvent(TRequest request, TResult result)
    {
        return default;
    }
}

public class BaseRequestHandler<TRequest> : IRequestHandler<TRequest>
{
    public virtual Task Validate(RequestValidationContext<TRequest> context)
    {
        return Task.CompletedTask;
    }

    public virtual Task Handle(TRequest request)
    {
        throw new NotImplementedException();
    }

    public virtual IEvent? GetEvent(TRequest request)
    {
        return default;
    }
}