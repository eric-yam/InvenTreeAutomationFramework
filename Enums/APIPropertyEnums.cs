namespace InvenTreeAutomationFramework.Enums;

public enum APIPropertyEnums
{
    Token,
    Results,
    PK,
    PartDetail,
    Reference
}

public static class APIProperty
{
    public static Dictionary<APIPropertyEnums, string> APIPropertyDictionary = new Dictionary<APIPropertyEnums, string>()
    {
        { APIPropertyEnums.Token, "token"},
        { APIPropertyEnums.Results, "results"},
        { APIPropertyEnums.PK, "pk"},
        { APIPropertyEnums.PartDetail, "part_detail"},
        { APIPropertyEnums.Reference, "reference"},
    };
}