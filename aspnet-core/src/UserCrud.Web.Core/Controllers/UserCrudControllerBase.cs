using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace UserCrud.Controllers
{
    public abstract class UserCrudControllerBase : AbpController
    {
        protected UserCrudControllerBase()
        {
            LocalizationSourceName = UserCrudConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
