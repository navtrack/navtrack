using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(IRequestHandler<UpdateUserRequest>))]
public class UpdateUserRequestHandler(IUserRepository userRepository, INavtrackContextAccessor navtrackContextAccessor)
    : BaseRequestHandler<UpdateUserRequest>
{
    public override async Task Handle(UpdateUserRequest request)
    {
        UpdateUser updateUser = new();

        if (!string.IsNullOrEmpty(request.Model.Email))
        {
            request.Model.Email = request.Model.Email.ToLower();

            if (navtrackContextAccessor.NavtrackContext.User.Email != request.Model.Email)
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
            navtrackContextAccessor.NavtrackContext.User.UnitsType != request.Model.UnitsType)
        {
            updateUser.UnitsType = request.Model.UnitsType;
        }

        await userRepository.Update(navtrackContextAccessor.NavtrackContext.User.Id, updateUser);
    }
}