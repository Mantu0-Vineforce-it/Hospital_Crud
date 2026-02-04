using Abp.Authorization;
using Abp.Runtime.Session;
using UserCrud.Configuration.Dto;
using System.Threading.Tasks;

namespace UserCrud.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : UserCrudAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
