using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.ManufacturingSection;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Home;

public class HomePage : AppPage
{
    private ManufacturingSectionTab manufacturingSectionTab;

    public HomePage(IPage page) : base(page)
    {
        this.manufacturingSectionTab = new ManufacturingSectionTab(this.page);
    }

    public ManufacturingSectionTab GetManufacturingTab()
    {
        return this.manufacturingSectionTab;
    }
}