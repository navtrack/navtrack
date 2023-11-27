using System.Threading.Tasks;
using Navtrack.Shared.Services.Email.Emails;

namespace Navtrack.Shared.Services.Email;

public interface IEmailService
{
    Task Send(string destination, IEmail email);
}