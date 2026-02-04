using Abp.Application.Services;
using UserCrud.Sessions.Dto;
using System.Threading.Tasks;

namespace UserCrud.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
