using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Forms;

public abstract class BaseForm
{
    private ILocator CloseWindowButton() => this.page.Locator("button[data-variant='subtle']");
    private ILocator SubmitButton() => this.page.Locator("button[data-variant='filled']");
    private ILocator CancelButton() => this.page.Locator("button[data-variant='outline']");
    protected IPage page;
    public BaseForm(IPage page)
    {
        this.page = page;
    }
    public async Task ClickCloseWindowButton() { await this.CloseWindowButton().ClickAsync(); }
    public async Task ClickSubmitButton() { await this.SubmitButton().ClickAsync(); }
    public async Task ClickCancelButton() { await this.CancelButton().ClickAsync(); }
}