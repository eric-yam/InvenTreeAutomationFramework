using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages;

public abstract class BasePage
{
    protected IPage page;
    public BasePage(IPage page)
    {
        this.page = page;
    }
}