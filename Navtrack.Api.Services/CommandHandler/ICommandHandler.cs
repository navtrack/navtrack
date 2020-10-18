using System.Threading.Tasks;
using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Services.CommandHandler
{
    public interface ICommandHandler
    {
        ApiResponseModel ApiResponse { get; }
    }
    
    public interface ICommandHandler<in TCommand> : ICommandHandler
    {
        Task Authorize(TCommand command);
        Task Validate(TCommand command);
        Task Handle(TCommand command);
    }
    
    public interface ICommandHandler<in TCommand, TResponseModel> : ICommandHandler
    {
        Task Authorize(TCommand command);
        Task Validate(TCommand command);
        Task<TResponseModel> Handle(TCommand command);
    }
}