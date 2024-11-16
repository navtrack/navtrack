using Navtrack.Api.Services.Common.Passwords;
using Xunit;

namespace Navtrack.Shared.Services.Tests.Passwords;

public class PasswordHasherTests
{
    private readonly IPasswordHasher passwordHasher;

    public PasswordHasherTests()
    {
        passwordHasher = new PasswordHasher();
    }
    
    [Fact]
    public void CheckPassword_SaltHashWithCorrectPasswordGiven_VerificationPassed()
    {
        const string hash = "CYZPZEXIPw9z7L+hPk4WrztnTG9BIelD47GLqOMWK0YVhLtcdJSu5viexW/WeOj0bwyUazamLB8MDa+j5FqjwQ==";
        const string salt = "KAiyKQ3m0q2IbLMNSfUxLvgUdGMYpnbTh0lAHpWA9VM=";
        const string password = "one two three";

        bool verified = passwordHasher.CheckPassword(password, hash, salt);

        Assert.True(verified);
    }
        
    [Fact]
    public void CheckPassword_SaltHashWithWrongPasswordGiven_VerificationFailed()
    {
        const string hash = "CYZPZEXIPw9z7L+hPk4WrztnTG9BIelD47GLqOMWK0YVhLtcdJSu5viexW/WeOj0bwyUazamLB8MDa+j5FqjwQ==";
        const string salt = "KAiyKQ3m0q2IbLMNSfUxLvgUdGMYpnbTh0lAHpWA9VM=";
        const string password = "one two three four";

        bool verified = passwordHasher.CheckPassword(password, hash, salt);

        Assert.False(verified);
    }
}