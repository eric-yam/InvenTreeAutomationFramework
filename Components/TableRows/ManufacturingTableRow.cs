using InvenTreeAutomationFramework.Enums;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components.TableRows;

public class ManufacturingTableRow : TableRow
{
    public ManufacturingTableRow(IPage page) : base(page) { }

    public override string GetKey()
    {
        return this.Row[TableRowKey.TableRowKeyDictionary[TableRowKeyEnumns.Manufacturing]];
    }
}