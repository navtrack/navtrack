namespace Navtrack.Common.Passwords;

public interface IPasswordHasher
{
    (string, string) Hash(string password);
    bool CheckPassword(string password, string hash, string salt);
}