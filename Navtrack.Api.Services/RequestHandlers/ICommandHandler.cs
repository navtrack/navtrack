using System.Threading.Tasks;
using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Services.RequestHandlers
{
    public interface IRequestHandler
    {
        ApiResponseModel ApiResponse { get; }
    }
    
    public interface ICommandHandler<in TCommand> : IRequestHandler
    {
        Task Authorize(TCommand request);
        Task Validate(TCommand request);
        Task Handle(TCommand request);
    }
    
    public interface IRequestHandler<in TCommand, TResponseModel> : IRequestHandler
    {
        Task Authorize(TCommand request);
        Task Validate(TCommand request);
        Task<TResponseModel> Handle(TCommand request);
    }
}