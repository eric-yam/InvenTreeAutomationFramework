using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Components;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages;

public abstract class AppPage : BasePage
{
    private HomeTabStrip homeTabStrip;
    public AppPage(IPage page) : base(page)
    {
        this.homeTabStrip = new HomeTabStrip(this.page);
    }

    //Common page actions
    [AllureStep("Tab {name} has been selected")]
    public async Task SelectTab(string name)
    {
        await this.homeTabStrip.SetTabstrip();
        await this.homeTabStrip.GetTabStripButtonlist()[name].ClickAsync();
    }
}