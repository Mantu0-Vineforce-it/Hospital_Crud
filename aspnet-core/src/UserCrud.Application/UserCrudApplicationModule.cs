using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using UserCrud.Authorization;

namespace UserCrud;

[DependsOn(
    typeof(UserCrudCoreModule),
    typeof(AbpAutoMapperModule))]
public class UserCrudApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<UserCrudAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(UserCrudApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
