using InvenTreeAutomationFramework.Enums;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components.TableRows;

public class StockItemsTableRow : TableRow
{
    public StockItemsTableRow(IPage page) : base(page) { }

    public override string GetKey()
    {
        //Appended stock item part column to create unique key for table
        return this.Row[TableRowKey.TableRowKeyDictionary[TableRowKeyEnumns.StockItems]] + "|" + this.Row[TableRowKey.TableRowKeyDictionary[TableRowKeyEnumns.StockItemsPart]];
    }
}