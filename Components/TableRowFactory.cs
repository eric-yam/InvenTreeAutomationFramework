using System.Threading.Tasks;
using InvenTreeAutomationFramework.Components.TableRows;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components;

public static class TableRowFactory
{
    // ================================== Manufacturing ==================================
    public static async Task<TableRow> InitManufacturingTableRow(IPage page, ILocator currentRow)
    {
        ManufacturingTableRow mtr = new ManufacturingTableRow(page);
        await mtr.InitializeRow(currentRow);
        return mtr;
    }
}