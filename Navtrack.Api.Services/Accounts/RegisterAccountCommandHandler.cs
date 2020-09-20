using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Navtrack.Api.Model.Accounts;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Accounts
{
    [Service(typeof(ICommandHandler<RegisterAccountModel>))]
    public class RegisterAccountCommandHandler : BaseCommandHandler<RegisterAccountModel>
    {
        private readonly IUserDataService userDataService;
        private readonly IMapper mapper;

        public RegisterAccountCommandHandler(IUserDataService userDataService, IMapper mapper)
        {
            this.userDataService = userDataService;
            this.mapper = mapper;
        }

        public override async Task Validate(RegisterAccountModel request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                ApiResponse.AddError(nameof(RegisterAccountModel.Email), "The email is required.");
            }
            else if (!new EmailAddressAttribute().IsValid(request.Email))
            {
                ApiResponse.AddError(nameof(RegisterAccountModel.Email), "The email address is not valid.");
            }
            else if (await userDataService.EmailIsUsed(request.Email))
            {
                ApiResponse.AddError(nameof(RegisterAccountModel.Email), "Email is already used.");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                ApiResponse.AddError(nameof(RegisterAccountModel.Password), "The password is required.");
            }
            if (string.IsNullOrEmpty(request.ConfirmPassword))
            {
                ApiResponse.AddError(nameof(RegisterAccountModel.ConfirmPassword), "The confirm password is required.");
            }
            else if (request.Password != request.ConfirmPassword)
            {
                ApiResponse.AddError(nameof(RegisterAccountModel.ConfirmPassword), "The passwords must match.");
            }
        }

        public override async Task Handle(RegisterAccountModel request)
        {
            UserEntity user = mapper.Map<RegisterAccountModel, UserEntity>(request);

            await userDataService.Add(user);
        }
    }
}