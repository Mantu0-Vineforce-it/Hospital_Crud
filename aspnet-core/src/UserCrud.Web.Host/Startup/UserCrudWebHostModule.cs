using Abp.Modules;
using Abp.Reflection.Extensions;
using UserCrud.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace UserCrud.Web.Host.Startup
{
    [DependsOn(
       typeof(UserCrudWebCoreModule))]
    public class UserCrudWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public UserCrudWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(UserCrudWebHostModule).GetAssembly());
        }
    }
}
