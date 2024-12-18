using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Model.Account;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Account.Mappers;
using Navtrack.Api.Services.Common.Email;
using Navtrack.Api.Services.Common.Email.Emails;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Settings;
using Navtrack.Api.Services.Common.Settings.Models;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Model.Users.PasswordResets;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Account;

[Service(typeof(IRequestHandler<ForgotPasswordRequest>))]
public class ForgotPasswordRequestHandler(
    IHttpContextAccessor httpContextAccessor,
    IPasswordResetRepository passwordResetRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    ISettingService settingService) : BaseRequestHandler<ForgotPasswordRequest>
{
    private UserDocument? user;
    private string? ipAddress;
    
    public override async Task Validate(RequestValidationContext<ForgotPasswordRequest> context)
    {
        ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        ipAddress.ReturnIfNull(HttpStatusCode.BadRequest);

        int passwordResetsIn24H =
            await passwordResetRepository.GetCountOfPasswordResets(ipAddress!, context.Request.Model.Email,
                DateTime.UtcNow.AddDays(-1));
        
        if (passwordResetsIn24H >= ApiConstants.MaxPasswordResetIn24Hours)
        {
            throw new ValidationApiException(nameof(ForgotPassword.Email),
                ApiErrorCodes.User_000005_MaxPasswordResetsExceeded);
        }

        user = await userRepository.GetByEmail(context.Request.Model.Email);
        user.ReturnValidationErrorIfNull(nameof(ForgotPassword.Email),
            ApiErrorCodes.User_000001_EmailNotFound);
    }

    public override async Task Handle(ForgotPasswordRequest request)
    {
        await passwordResetRepository.MarkAsInvalidByUserId(user.Id);

        PasswordResetDocument document =
            PasswordResetDocumentMapper.Map(request.Model.Email, user.Id, ipAddress!);

        await passwordResetRepository.Add(document);

        string url = await GetResetPasswordUrl(document);

        await emailService.Send(document.Email, new ResetPasswordEmail(url, 3));
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