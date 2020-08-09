using System.Threading.Tasks;
using Navtrack.Api.Model;

namespace Navtrack.Api.Services.RequestHandlers
{
    public interface IRequestHandler
    {
        ApiResponseModel ApiResponse { get; }
    }
    
    public interface IRequestHandler<in TRequest> : IRequestHandler
    {
        Task Authorize(TRequest request);
        Task Validate(TRequest request);
        Task Handle(TRequest request);
    }
    
    public interface IRequestHandler<in TRequest, TResponse> : IRequestHandler
    {
        Task Authorize(TRequest request);
        Task Validate(TRequest request);
        Task<TResponse> Handle(TRequest request);
    }
}