namespace InvenTreeAutomationFramework.Enums;

public enum TableRowKeyEnumns
{
    Manufacturing,
    StockItems,
    StockItemsPart
}

public static class TableRowKey
{
    public static Dictionary<TableRowKeyEnumns, string> TableRowKeyDictionary = new Dictionary<TableRowKeyEnumns, string>()
    {
        { TableRowKeyEnumns.Manufacturing, "Reference"},
        { TableRowKeyEnumns.StockItems, "Stock"},
        { TableRowKeyEnumns.StockItemsPart, "Part"},
    };
}