using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using UserCrud.MultiTenancy;

namespace UserCrud.Sessions.Dto;

[AutoMapFrom(typeof(Tenant))]
public class TenantLoginInfoDto : EntityDto
{
    public string TenancyName { get; set; }

    public string Name { get; set; }
}
