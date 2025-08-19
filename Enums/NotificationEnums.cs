namespace InvenTreeAutomationFramework.Enums;

public enum NotificationEnums
{
    LoginSuccess,
    LoginFail,
    LogoutSuccess,
    SuccessItemCreated,
    SuccessItemUpdated
}

public static class Notifications
{
    public static Dictionary<NotificationEnums, string> NotificationDictionary = new Dictionary<NotificationEnums, string>()
    {
        { NotificationEnums.LoginSuccess, "Login successful Logged in successfully"},
        { NotificationEnums.LoginFail, "Login failed Check your input and try again."},
        { NotificationEnums.LogoutSuccess, "Logged Out Successfully logged out"},
        { NotificationEnums.SuccessItemCreated, "Success Item Created"},
        { NotificationEnums.SuccessItemUpdated, "Success Item Updated"}
    };
}