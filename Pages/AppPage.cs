using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Components;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages;

public abstract class AppPage : BasePage
{
    //Locators
    private ILocator AppTitle() => this.page.Locator("div[class*='mantine-Container-root'] p[class*='mantine-Text-root'] h6");

    //Instance variables
    private HomeTabStrip homeTabStrip;

    public AppPage(IPage page) : base(page)
    {
        this.homeTabStrip = new HomeTabStrip(this.page);
    }

    //Common page actions
    [AllureStep("Tab [{name}] Has Been Selected")]
    public async Task SelectTab([Skip] string name)
    {
        await this.homeTabStrip.SetTabstrip();
        await this.homeTabStrip.GetTabStripButtonlist()[name].ClickAsync();
    }

    [AllureStep("Application Title Visible")]
    public async Task<bool> AppTitleDisplayed()
    {
        await this.AppTitle().WaitForAsync();
        return await this.AppTitle().IsVisibleAsync();
    }
}