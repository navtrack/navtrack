using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Model.General;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.IdentityServer;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.User
{
    [Service(typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IUserDataService userDataService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor, IUserDataService userDataService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userDataService = userDataService;
        }

        public async Task<UserModel> GetCurrentUser()
        {
            int currentUserId = httpContextAccessor.HttpContext.User.GetId();

            UserEntity entity = await userDataService.GetUserById(currentUserId);

            return new UserModel
            {
                Id = entity.Id,
                Email = entity.Email,
                MeasurementSystem = (MeasurementSystem) entity.MeasurementSystem
            };
        }
    }
}