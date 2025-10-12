using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Account.Events;
using Navtrack.Api.Services.Account.Mappers;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Account;

[Service(typeof(IRequestHandler<CreateAccountRequest>))]
public class CreateAccountRequestHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ICaptchaValidator? captchaValidator = null) : BaseRequestHandler<CreateAccountRequest>
{
    private UserEntity user;
    
    public override async Task Validate(RequestValidationContext<CreateAccountRequest> context)
    {
        context.ValidationException.AddErrorIfTrue(await userRepository.EmailIsUsed(context.Request.Model.Email),
            nameof(context.Request.Model.Email),
            ApiErrorCodes.User_000002_EmailAlreadyUsed);

        context.ValidationException.AddErrorIfTrue(
            context.Request.Model.Password != context.Request.Model.ConfirmPassword,
            nameof(context.Request.Model.ConfirmPassword),
            ApiErrorCodes.User_000003_PasswordsNotEqual);

        context.ValidationException.AddErrorIfTrue(
            captchaValidator != null && !await captchaValidator.Validate(context.Request.Model.Captcha),
            nameof(context.Request.Model.Captcha),
            ApiErrorCodes.Validation_000002_InvalidCaptcha);
    }

    public override async Task Handle(CreateAccountRequest request)
    {
        (string hash, string salt) = passwordHasher.Hash(request.Model.Password);

        user = UserDocumentMapper.Map(request.Model.Email, hash, salt);
        
        await userRepository.Add(user);
    }

    public override IEvent GetEvent(CreateAccountRequest request)
    {
        return new AccountCreatedEvent
        {
            UserId = user.Id.ToString()
        };
    }
}