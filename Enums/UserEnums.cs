namespace InvenTreeAutomationFramework.Enums;

public enum UserEnums
{
    AllAccess,
    Reader,
    Engineer,
    Admin
}

public static class UserRoles
{
    public static Dictionary<UserEnums, string> RolesDict = new Dictionary<UserEnums, string>()
    {
        { UserEnums.AllAccess, "all access" },
        { UserEnums.Reader, "reader" },
        { UserEnums.Engineer, "engineer" },
        { UserEnums.Admin, "admin"}
    };
}