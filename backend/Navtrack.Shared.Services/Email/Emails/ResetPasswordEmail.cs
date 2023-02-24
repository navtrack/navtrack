namespace Navtrack.Common.Email.Emails;

public class ResetPasswordEmail : IEmail
{
    private readonly string link;
    private readonly int expirationHours;

    public ResetPasswordEmail(string link, int expirationHours)
    {
        this.link = link;
        this.expirationHours = expirationHours;
    }

    public string Subject => "[Navtrack] Reset password";

    public string Body => $$"""
                Hello,<br /><br />
                    
                If you requested to reset your password, continue the process by clicking on the link below:<br />            
                <a href="{{link}}">{{link}}</a><br /><br />

                If you don't use the link within {{expirationHours}} hours, it will expire.<br /><br />

                Thanks,<br />
                The Navtrack Team
               """;
}