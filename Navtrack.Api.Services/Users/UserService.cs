using System.Threading.Tasks;
using Navtrack.Api.Model.Users;
using Navtrack.Api.Services.Mappers;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;
using UnitsType = Navtrack.DataAccess.Model.Common.UnitsType;

namespace Navtrack.Api.Services.Users;

[Service(typeof(IUserService))]
public class UserService : IUserService
{
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly IUserDataService userDataService;

    public UserService(ICurrentUserAccessor currentUserAccessor, IUserDataService userDataService)
    {
        this.currentUserAccessor = currentUserAccessor;
        this.userDataService = userDataService;
    }

    public async Task<CurrentUserModel> GetCurrentUser()
    {
        UserDocument entity = await currentUserAccessor.GetCurrentUser();

        return CurrentUserMapper.Map(entity);
    }

    public async Task UpdateUser(UpdateUserModel model)
    {
        UserDocument currentUser = await currentUserAccessor.GetCurrentUser();

        await userDataService.UpdateUser(currentUser, model.Email, (UnitsType?)model.UnitsType);
    }
}