using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using UserCrud.Configuration;
using UserCrud.EntityFrameworkCore;
using UserCrud.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace UserCrud.Migrator;

[DependsOn(typeof(UserCrudEntityFrameworkModule))]
public class UserCrudMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public UserCrudMigratorModule(UserCrudEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(UserCrudMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            UserCrudConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(UserCrudMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
