using InvenTreeAutomationFramework.Pages.SideBarTabs.Manufacturing;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.SectionTabs;

public class ManufacturingSectionTab : BaseSectionTab
{
    private BuildOrderTab buildOrderTab;
    public ManufacturingSectionTab(IPage page) : base(page)
    {
        this.buildOrderTab = new BuildOrderTab(this.page);
    }

    public BuildOrderTab GetBuildOrderTab()
    {
        return this.buildOrderTab;
    }
}