using System.Threading.Tasks;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Requests;

public interface IRequestHandler
{
    Task Handle<TRequest>(TRequest request);
    Task<TResult> Handle<TRequest, TResult>(TRequest request);
}

public interface IRequestHandler<TRequest, TResult>
{
    Task Validate(RequestValidationContext<TRequest> context);
    Task<TResult> Handle(TRequest request);
    IEvent? GetEvent(TRequest request, TResult result);
}

public interface IRequestHandler<TRequest>
{
    Task Validate(RequestValidationContext<TRequest> context);
    Task Handle(TRequest request);
    IEvent? GetEvent(TRequest request);
}