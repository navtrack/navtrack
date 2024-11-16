using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Email.Emails;

namespace Navtrack.Api.Services.Common.Email;

public interface IEmailService
{
    Task Send(string destination, IEmail email);
}