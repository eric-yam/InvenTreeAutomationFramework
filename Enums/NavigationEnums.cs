namespace InvenTreeAutomationFramework.Enums;

public enum NavigationEnums
{
    Parts,
    Stock,
    Manufacturing,
    Purchasing,
    Sales
}

public static class Navigation
{
    public static Dictionary<NavigationEnums, string> NavigationSteps = new Dictionary<NavigationEnums, string>()
    {
        { NavigationEnums.Parts, "Parts"},
        { NavigationEnums.Stock, "Stock"},
        { NavigationEnums.Manufacturing, "Manufacturing"},
        { NavigationEnums.Purchasing, "Purchasing"},
        { NavigationEnums.Sales, "Sales"},
    };
}