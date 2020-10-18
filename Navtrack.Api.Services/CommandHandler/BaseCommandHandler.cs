using System;
using System.Threading.Tasks;
using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Services.CommandHandler
{
    public abstract class BaseCommandHandler : ICommandHandler
    {
        public ApiResponseModel ApiResponse { get; }

        protected BaseCommandHandler()
        {
            ApiResponse = new ApiResponseModel();
        }
    }

    public abstract class BaseCommandHandler<TCommand> : BaseCommandHandler, ICommandHandler<TCommand>
    {
        public virtual Task Authorize(TCommand command)
        {
            return Task.CompletedTask;
        }

        public virtual Task Validate(TCommand command)
        {
            return Task.CompletedTask;
        }

        public virtual Task Handle(TCommand command)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class
        BaseCommandHandler<TCommand, TResponseModel> : BaseCommandHandler,
            ICommandHandler<TCommand, TResponseModel>
    {
        public virtual Task Authorize(TCommand command)
        {
            return Task.CompletedTask;
        }

        public virtual Task Validate(TCommand command)
        {
            return Task.CompletedTask;
        }

        public virtual Task<TResponseModel> Handle(TCommand command)
        {
            throw new NotImplementedException();
        }
    }
}