using Abp.Authorization;
using UserCrud.Authorization.Roles;
using UserCrud.Authorization.Users;

namespace UserCrud.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
