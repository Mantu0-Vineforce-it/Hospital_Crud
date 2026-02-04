using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using UserCrud.EntityFrameworkCore;
using UserCrud.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace UserCrud.Web.Tests;

[DependsOn(
    typeof(UserCrudWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class UserCrudWebTestModule : AbpModule
{
    public UserCrudWebTestModule(UserCrudEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(UserCrudWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(UserCrudWebMvcModule).Assembly);
    }
}