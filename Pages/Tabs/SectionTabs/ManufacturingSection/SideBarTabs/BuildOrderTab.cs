using InvenTreeAutomationFramework.Components;
using InvenTreeAutomationFramework.Pages.SectionTabs;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.ManufacturingSection.SideBarTabs;

public class BuildOrderTab : BaseInvenTablePage
{
    public BuildOrderTab(IPage page) : base(page) { }

    public async Task InitializeTable()
    {
        await this.table.InitializeTable(TableRowFactory.InitManufacturingTableRow);
    }
}