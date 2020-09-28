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

    public abstract class BaseCommandHandler<TRequestModel> : BaseCommandHandler, ICommandHandler<TRequestModel>
    {
        public virtual Task Authorize(TRequestModel request)
        {
            return Task.CompletedTask;
        }

        public virtual Task Validate(TRequestModel request)
        {
            return Task.CompletedTask;
        }

        public virtual Task Handle(TRequestModel request)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class
        BaseCommandHandler<TRequestModel, TResponseModel> : BaseCommandHandler,
            ICommandHandler<TRequestModel, TResponseModel>
    {
        public virtual Task Authorize(TRequestModel request)
        {
            return Task.CompletedTask;
        }

        public virtual Task Validate(TRequestModel request)
        {
            return Task.CompletedTask;
        }

        public virtual Task<TResponseModel> Handle(TRequestModel request)
        {
            throw new NotImplementedException();
        }
    }
}