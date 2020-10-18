using System.Threading.Tasks;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.User
{
    [Service(typeof(ICommandHandler<GetCurrentUserCommand, UserResponseModel>))]
    public class GetCurrentUserCommandHandler : BaseCommandHandler<GetCurrentUserCommand, UserResponseModel>
    {
        private readonly IUserService userService;

        public GetCurrentUserCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public override Task<UserResponseModel> Handle(GetCurrentUserCommand command)
        {
            return userService.GetCurrentUser();
        }
    }
}