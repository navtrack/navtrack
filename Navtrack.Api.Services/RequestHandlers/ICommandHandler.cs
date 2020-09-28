using System.Threading.Tasks;
using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Services.RequestHandlers
{
    public interface ICommandHandler
    {
        ApiResponseModel ApiResponse { get; }
    }
    
    public interface ICommandHandler<in TCommand> : ICommandHandler
    {
        Task Authorize(TCommand request);
        Task Validate(TCommand request);
        Task Handle(TCommand request);
    }
    
    public interface ICommandHandler<in TCommand, TResponseModel> : ICommandHandler
    {
        Task Authorize(TCommand request);
        Task Validate(TCommand request);
        Task<TResponseModel> Handle(TCommand request);
    }
}