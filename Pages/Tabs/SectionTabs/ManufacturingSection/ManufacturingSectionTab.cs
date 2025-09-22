using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.ManufacturingSection.SideBarTabs;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.ManufacturingSection;

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