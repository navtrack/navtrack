using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Navtrack.Api.Model.Accounts;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Accounts
{
    [Service(typeof(ICommandHandler<RegisterAccountRequestModel>))]
    public class RegisterAccountCommandHandler : BaseCommandHandler<RegisterAccountRequestModel>
    {
        private readonly IUserDataService userDataService;
        private readonly IMapper mapper;

        public RegisterAccountCommandHandler(IUserDataService userDataService, IMapper mapper)
        {
            this.userDataService = userDataService;
            this.mapper = mapper;
        }

        public override async Task Validate(RegisterAccountRequestModel command)
        {
            if (string.IsNullOrEmpty(command.Email))
            {
                ApiResponse.AddError(nameof(RegisterAccountRequestModel.Email), "The email is required.");
            }
            else if (!new EmailAddressAttribute().IsValid(command.Email))
            {
                ApiResponse.AddError(nameof(RegisterAccountRequestModel.Email), "The email address is not valid.");
            }
            else if (await userDataService.EmailIsUsed(command.Email))
            {
                ApiResponse.AddError(nameof(RegisterAccountRequestModel.Email), "Email is already used.");
            }

            if (string.IsNullOrEmpty(command.Password))
            {
                ApiResponse.AddError(nameof(RegisterAccountRequestModel.Password), "The password is required.");
            }
            if (string.IsNullOrEmpty(command.ConfirmPassword))
            {
                ApiResponse.AddError(nameof(RegisterAccountRequestModel.ConfirmPassword), "The confirm password is required.");
            }
            else if (command.Password != command.ConfirmPassword)
            {
                ApiResponse.AddError(nameof(RegisterAccountRequestModel.ConfirmPassword), "The passwords must match.");
            }
        }

        public override async Task Handle(RegisterAccountRequestModel command)
        {
            UserEntity user = mapper.Map<RegisterAccountRequestModel, UserEntity>(command);

            await userDataService.Add(user);
        }
    }
}