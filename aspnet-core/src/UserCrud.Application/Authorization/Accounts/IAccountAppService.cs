using Abp.Application.Services;
using UserCrud.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace UserCrud.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}
