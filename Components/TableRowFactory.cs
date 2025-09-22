using System.Threading.Tasks;
using InvenTreeAutomationFramework.Components.TableRows;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components;

public static class TableRowFactory
{
    // ================================== Stocks =========================================
    public static async Task<TableRow> InitStockItemsTableRow(IPage page, ILocator currentRow)
    {
        StockItemsTableRow str = new StockItemsTableRow(page);
        await str.InitializeRow(currentRow);
        return str;
    }

    // ================================== Manufacturing ==================================
    public static async Task<TableRow> InitManufacturingTableRow(IPage page, ILocator currentRow)
    {
        ManufacturingTableRow mtr = new ManufacturingTableRow(page);
        await mtr.InitializeRow(currentRow);
        return mtr;
    }
}