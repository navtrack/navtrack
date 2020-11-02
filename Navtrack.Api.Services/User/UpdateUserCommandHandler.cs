using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.General;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.User
{
    [Service(typeof(ICommandHandler<UpdateUserCommand>))]
    public class UpdateUserCommandHandler : BaseCommandHandler<UpdateUserCommand>
    {
        private readonly IRepository repository;

        public UpdateUserCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public override Task Validate(UpdateUserCommand command)
        {
            if (!command.Model.MeasurementSystem.HasValue ||
                !Enum.IsDefined(typeof(MeasurementSystem), command.Model.MeasurementSystem.Value))
            {
                ApiResponse.AddError(nameof(command.Model.MeasurementSystem), "The value provided is not valid.");
            }

            return Task.CompletedTask;
        }

        public override async Task Handle(UpdateUserCommand command)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            UserEntity user = await unitOfWork.GetEntities<UserEntity>().FirstAsync(x => x.Id == command.UserId);

            if (command.Model.MeasurementSystem.HasValue)
            {
                user.MeasurementSystem = (int) command.Model.MeasurementSystem.Value;
            }

            unitOfWork.Update(user);
            await unitOfWork.SaveChanges();
        }
    }
}