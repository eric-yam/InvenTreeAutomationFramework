namespace InvenTreeAutomationFramework.Enums;

public enum APIHelperEnums
{
    Root,
    UserMe
}

public static class APIEndpoints
{
    public const string COMMON_TABLE_PAYLOAD = "detail=true&limit=25&offset=0";
    public static Dictionary<APIHelperEnums, string> APIEndpointDictionary = new Dictionary<APIHelperEnums, string>()
    {
        { APIHelperEnums.Root, "/api/"},
        { APIHelperEnums.UserMe, "/api/user/me/"}
    };    
}