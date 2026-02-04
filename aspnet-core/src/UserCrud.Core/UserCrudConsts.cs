using UserCrud.Debugging;

namespace UserCrud;

public class UserCrudConsts
{
    public const string LocalizationSourceName = "UserCrud";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "26ba1ad922424cb384a7191e5977b389";
}
