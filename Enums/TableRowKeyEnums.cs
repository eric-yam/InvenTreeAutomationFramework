namespace InvenTreeAutomationFramework.Enums;

public enum TableRowKeyEnumns
{
    Manufacturing
}

public static class TableRowKey
{
    public static Dictionary<TableRowKeyEnumns, string> TableRowKeyDictionary = new Dictionary<TableRowKeyEnumns, string>()
    {
        { TableRowKeyEnumns.Manufacturing, "Reference"}
    };
}