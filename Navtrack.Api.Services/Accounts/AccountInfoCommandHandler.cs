using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Accounts;
using Navtrack.Api.Model.Accounts.Requests;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Accounts
{
    [Service(typeof(IRequestHandler<AccountInfoCommand, AccountInfoResponseModel>))]
    public class AccountInfoCommandHandler : BaseRequestHandler<AccountInfoCommand, AccountInfoResponseModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public AccountInfoCommandHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override async Task<AccountInfoResponseModel> Handle(AccountInfoCommand command)
        {
            UserEntity entity =
                await repository.GetEntities<UserEntity>().FirstOrDefaultAsync(x => x.Id == command.UserId);

            if (entity != null)
            {
                AccountInfoResponseModel model = mapper.Map<UserEntity, AccountInfoResponseModel>(entity);

                return model;
            }

            return null;
        }
    }
}