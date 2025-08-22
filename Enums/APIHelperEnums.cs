namespace InvenTreeAutomationFramework.Enums;

public enum APIHelperEnums
{
    Token,
    Root,
    UserMe,
    ProfileLanguage
}

public static class APIEndpoints
{
    public const string COMMON_TABLE_PAYLOAD = "detail=true&limit=25&offset=0";
    public const string COMMON_ORDER_DETAIL_PAYLOAD = "_detail=true";
    public static Dictionary<APIHelperEnums, string> APIEndpointDictionary = new Dictionary<APIHelperEnums, string>()
    {
        { APIHelperEnums.Token, "/api/user/token/"},
        { APIHelperEnums.Root, "/api/"},
        { APIHelperEnums.UserMe, "/api/user/me/"},
        { APIHelperEnums.ProfileLanguage, "/api/user/profile/"}
    };
}