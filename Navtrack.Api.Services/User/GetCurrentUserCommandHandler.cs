using System.Threading.Tasks;
using Navtrack.Api.Model.Commands;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.User
{
    [Service(typeof(ICommandHandler<GetCurrentUserCommand, UserModel>))]
    public class GetCurrentUserCommandHandler : BaseCommandHandler<GetCurrentUserCommand, UserModel>
    {
        private readonly IUserService userService;

        public GetCurrentUserCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public override Task<UserModel> Handle(GetCurrentUserCommand command)
        {
            return userService.GetCurrentUser();
        }
    }
}