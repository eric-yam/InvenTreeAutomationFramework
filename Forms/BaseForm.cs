using Allure.NUnit.Attributes;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Forms;

public abstract class BaseForm
{
    private ILocator CloseWindowButton() => this.page.Locator("button[data-variant='subtle']");
    private ILocator SubmitButton() => this.page.Locator("button[class*='mantine-Button-root'][data-variant='filled']");
    private ILocator CancelButton() => this.page.Locator("button[data-variant='outline']");
    protected IPage page;
    
    public BaseForm(IPage page)
    {
        this.page = page;
    }

    [AllureStep("User Closes Form Via X Button")]
    public async Task ClickCloseWindowButton() { await this.CloseWindowButton().ClickAsync(); }

    [AllureStep("User Clicks Submit Button On Form")]
    public async Task ClickSubmitButton() { await this.SubmitButton().ClickAsync(); }

    [AllureStep("User Clicks Cancel Button On Form")]
    public async Task ClickCancelButton() { await this.CancelButton().ClickAsync(); }
}