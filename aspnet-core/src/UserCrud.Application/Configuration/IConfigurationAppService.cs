using UserCrud.Configuration.Dto;
using System.Threading.Tasks;

namespace UserCrud.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
