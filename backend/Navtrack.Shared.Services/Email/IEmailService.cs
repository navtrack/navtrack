using System.Threading.Tasks;
using Navtrack.Common.Email.Emails;

namespace Navtrack.Common.Email;

public interface IEmailService
{
    Task Send(string destination, IEmail email);
}