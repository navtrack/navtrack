using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Extensions;
using Navtrack.Api.Services.Mappers;
using Navtrack.Common.Email;
using Navtrack.Common.Email.Emails;
using Navtrack.Common.Passwords;
using Navtrack.Common.Settings;
using Navtrack.Common.Settings.Settings;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(IUserAccessService))]
public class UserAccessService : IUserAccessService
{
    private readonly IPasswordHasher passwordHasher;
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IEmailService emailService;
    private readonly ISettingService settingService;
    private readonly IUserDataService userDataService;
    private readonly IPasswordResetDataService passwordResetDataService;

    public UserAccessService(IPasswordHasher passwordHasher, ICurrentUserAccessor currentUserAccessor,
        IHttpContextAccessor httpContextAccessor, IEmailService emailService, ISettingService settingService,
        IUserDataService userDataService, IPasswordResetDataService passwordResetDataService)
    {
        this.passwordHasher = passwordHasher;
        this.currentUserAccessor = currentUserAccessor;
        this.httpContextAccessor = httpContextAccessor;
        this.emailService = emailService;
        this.settingService = settingService;
        this.userDataService = userDataService;
        this.passwordResetDataService = passwordResetDataService;
    }

    public async Task ForgotPassword(ForgotPasswordRequest model)
    {
        string? ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        ipAddress.ThrowApiExceptionIfNullOrEmpty(ApiErrorCodes.InvalidIpAddress);

        int passwordResetsIn24H =
            await passwordResetDataService.GetCountOfPasswordResets(ipAddress!, DateTime.UtcNow.AddDays(-1));

        if (passwordResetsIn24H >= ApiConstants.MaxPasswordResetIn24Hours)
        {
            throw new ApiException(ApiErrorCodes.MaxPasswordResetsExceeded, HttpStatusCode.TooManyRequests);
        }

        UserDocument? userDocument = await userDataService.GetUserByEmail(model.Email);

        if (userDocument == null)
        {
            throw new ApiException(ApiErrorCodes.Validation)
                .AddValidationError(nameof(ForgotPasswordRequest.Email), ValidationErrorCodes.EmailDoesNotExist);
        }

        await passwordResetDataService.MarkAsInvalidByUserId(userDocument.Id);
        
        PasswordResetDocument document =
            PasswordResetMapper.Map(model.Email, userDocument.Id, ipAddress!);

        await passwordResetDataService.Add(document);

        string url = await GetResetPasswordUrl(document);

        await emailService.Send(document.Email, new ResetPasswordEmail(url, 3));
    }

    public async Task ChangePassword(ChangePasswordRequest model)
    {
        UserDocument currentUser = await currentUserAccessor.GetCurrentUser();

        currentUser.ThrowApiExceptionIfNull(HttpStatusCode.Unauthorized);

        ApiException apiException = new();

        if (!passwordHasher.CheckPassword(model.CurrentPassword, currentUser.Password.Hash,
                currentUser.Password.Salt))
        {
            apiException.AddValidationError(nameof(model.CurrentPassword), ValidationErrorCodes.CurrentPasswordInvalid);
        }

        ValidatePasswords(model, apiException, model.CurrentPassword);

        apiException.ThrowIfInvalid();

        (string hash, string salt) = passwordHasher.Hash(model.Password);

        await userDataService.ChangePassword(currentUser.Id, hash, salt);
    }

    public async Task ResetPassword(ResetPasswordRequest model)
    {
        ApiException apiException = new();

        ValidatePasswords(model, apiException);

        apiException.ThrowIfInvalid();

        PasswordResetDocument? passwordReset = await passwordResetDataService.GetLatestFromHash(model.Hash);

        if (passwordReset == null || passwordReset.Invalid || passwordReset.Hash != model.Hash)
        {
            throw new ApiException(ApiErrorCodes.PasswordResetInvalid);
        }

        if (passwordReset.Created.Date < DateTime.UtcNow.AddHours(-ApiConstants.PasswordResetLinkExpirationHours))
        {
            throw new ApiException(ApiErrorCodes.PasswordResetExpired);
        }

        (string hash, string salt) = passwordHasher.Hash(model.Password);

        await userDataService.ChangePassword(passwordReset.UserId, hash, salt);
        await passwordResetDataService.MarkAsInvalid(passwordReset.Id);
    }

    private static void ValidatePasswords(BasePasswordRequest model, ApiException apiException,
        string? currentPassword = null)
    {
        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword), ValidationErrorCodes.PasswordsDoNotMatch);
        }
        else if (model.Password == currentPassword)
        {
            apiException.AddValidationError(nameof(model.Password), ValidationErrorCodes.PasswordMustBeNew);
        }
    }

    private async Task<string> GetResetPasswordUrl(PasswordResetDocument document)
    {
        AppSettings? appSettings = await settingService.Get<AppSettings>();

        if (!string.IsNullOrEmpty(appSettings?.AppUrl))
        {
            return ApiConstants.ResetPasswordLink(appSettings.AppUrl, document.Hash);
        }

        throw new ArgumentException("App URL not set.");
    }
}