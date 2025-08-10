using InvenTreeAutomationFramework.Components;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages;

public abstract class BaseSectionTab : AppPage
{
    protected SideBar SectionSideBar;

    public BaseSectionTab(IPage page) : base(page)
    {
        this.SectionSideBar = new SideBar(this.page);
    }

    public async Task SelectSideBarTab(string name)
    {
        await this.SectionSideBar.SetSideBar();
        await this.SectionSideBar.GetSideBarButtonlist()[name].ClickAsync();
    }
}