using System.Threading.Tasks;

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
}

public interface IRequestHandler<TRequest>
{
    Task Validate(RequestValidationContext<TRequest> context);
    Task Handle(TRequest request);
}