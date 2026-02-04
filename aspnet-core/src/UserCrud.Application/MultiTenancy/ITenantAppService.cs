using Abp.Application.Services;
using UserCrud.MultiTenancy.Dto;

namespace UserCrud.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

