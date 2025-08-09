using InvenTreeAutomationFramework.Pages.SectionTabs;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Home;

public class HomePage : AppPage
{
    private ManufacturingTab manufacturingPage;
    public HomePage(IPage page) : base(page)
    {
        this.manufacturingPage = new ManufacturingTab(this.page);
    }

    public ManufacturingTab GetManufacturingTab()
    {
        return this.manufacturingPage;
    }
}