using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.ManufacturingSection;
using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.StockSection;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Home;

public class HomePage : AppPage
{
    private ManufacturingSectionTab manufacturingSectionTab;
    private StockSectionTab stockSectionTab;

    public HomePage(IPage page) : base(page)
    {
        this.manufacturingSectionTab = new ManufacturingSectionTab(this.page);
        this.stockSectionTab = new StockSectionTab(this.page);
    }

    public ManufacturingSectionTab GetManufacturingSectionTab()
    {
        return this.manufacturingSectionTab;
    }

    public StockSectionTab GetStockSectionTab()
    {
        return this.stockSectionTab;
    }
}