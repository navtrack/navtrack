namespace Navtrack.Api.Services.Common.Email.Emails;

public interface IEmail
{
    string Subject { get; }
    string Body { get; }
}