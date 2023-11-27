namespace Navtrack.Shared.Services.Email.Emails;

public interface IEmail
{
    string Subject { get; }
    string Body { get; }
}