using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(IRequestHandler<UpdateUserRequest>))]
public class UpdateUserRequestHandler(IUserRepository userRepository, INavtrackRequestContextAccessor navtrackRequestContextAccessor)
    : BaseRequestHandler<UpdateUserRequest>
{
    public override async Task Handle(UpdateUserRequest request)
    {
        UpdateUser updateUser = new();

        if (!string.IsNullOrEmpty(request.Model.Email))
        {
            request.Model.Email = request.Model.Email.ToLower();

            if (navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Email != request.Model.Email)
            {
                if (await userRepository.EmailIsUsed(request.Model.Email))
                {
                    throw new ValidationApiException().AddValidationError(nameof(UpdateUserModel.Email),
                        ApiErrorCodes.User_000002_EmailAlreadyUsed);
                }

                updateUser.Email = request.Model.Email;
            }
        }

        if (request.Model.UnitsType.HasValue &&
            navtrackRequestContextAccessor.NavtrackContext.CurrentUser.UnitsType != request.Model.UnitsType)
        {
            updateUser.UnitsType = request.Model.UnitsType;
        }

        await userRepository.Update(navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Id, updateUser);
    }
}