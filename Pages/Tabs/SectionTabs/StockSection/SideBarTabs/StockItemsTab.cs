using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Components;
using InvenTreeAutomationFramework.Pages.SectionTabs;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.StockSection.SideBarTabs;

public class StockItemsTab : BaseInvenTableTab
{
    public StockItemsTab(IPage page) : base(page) { }

    [AllureStep("Initialize Stock Items Tab Inventory Table")]
    public async Task InitializeTable()
    {
        await this.table.InitializeTable(TableRowFactory.InitStockItemsTableRow);
    }
}