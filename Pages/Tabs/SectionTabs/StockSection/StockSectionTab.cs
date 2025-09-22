using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.StockSection.SideBarTabs;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.StockSection;

public class StockSectionTab : BaseSectionTab
{
    private StockItemsTab stockItemsTab;

    public StockSectionTab(IPage page) : base(page)
    {
        this.stockItemsTab = new StockItemsTab(page);
    }

    public StockItemsTab GetStockItemsTab()
    {
        return this.stockItemsTab;
    }
}