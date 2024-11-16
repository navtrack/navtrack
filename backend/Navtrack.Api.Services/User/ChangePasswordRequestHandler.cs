using System.Net;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Account.Mappers;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(IRequestHandler<ChangePasswordRequest>))]
public class ChangePasswordRequestHandler(
    IUserRepository userRepository,
    INavtrackContextAccessor navtrackContextAccessor,
    IPasswordHasher passwordHasher)
    : BaseRequestHandler<ChangePasswordRequest>
{
    public override async Task Handle(ChangePasswordRequest request)
    {
        UserDocument? currentUser = navtrackContextAccessor.NavtrackContext?.User;
        currentUser.ThrowApiExceptionIfNull(HttpStatusCode.Unauthorized);

        ValidationApiException apiException = new();

        if (currentUser.Password != null && !passwordHasher.CheckPassword(request.Model.CurrentPassword,
                currentUser.Password.Hash,
                currentUser.Password.Salt))
        {
            apiException.AddValidationError(nameof(request.Model.CurrentPassword),
                ApiErrorCodes.User_000008_InvalidCurrentPassword);
        }

        BasePasswordModelValidator.ValidatePasswords(request.Model, apiException, request.Model.CurrentPassword);

        apiException.ThrowIfInvalid();

        (string hash, string salt) = passwordHasher.Hash(request.Model.Password);

        await userRepository.Update(currentUser.Id, new UpdateUser
        {
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });
    }
}