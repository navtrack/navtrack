using System.Threading.Tasks;
using Navtrack.Api.Models;
using Navtrack.Api.Services.Generic;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services
{
    [Service(typeof(IAccountService))]
    public class AccountService : IAccountService
    {
        private readonly IUserService userService;
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public AccountService(IUserService userService, IRepository repository, IMapper mapper)
        {
            this.userService = userService;
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ValidationResult> Register(RegisterModel registerModel)
        {
            ValidationResult validationResult = new ValidationResult();

            if (await userService.EmailIsUsed(registerModel.Email))
            {
                validationResult.AddError(nameof(RegisterModel.Email), "Email is already used.");
            }

            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                validationResult.AddError(nameof(RegisterModel.ConfirmPassword), "The passwords must match.");
            }

            if (validationResult.IsValid)
            {
                using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

                UserEntity user = mapper.Map<RegisterModel, UserEntity>(registerModel);
                
                unitOfWork.Add(user);

                await unitOfWork.SaveChanges();
            }

            return validationResult;
        }
    }
}