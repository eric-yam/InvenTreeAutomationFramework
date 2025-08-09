using InvenTreeAutomationFramework.Components;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.SectionTabs;

public abstract class BaseSectionTab : AppPage
{
    protected SectionSideBar SectionSideBar;

    public BaseSectionTab(IPage page) : base(page)
    {
        this.SectionSideBar = new SectionSideBar(this.page);
    }

    //Method for selecting side bar tab
}