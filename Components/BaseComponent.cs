using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components;

public abstract class BaseComponent
{
    protected IPage page;
    public BaseComponent(IPage page)
    {
        this.page = page;
    }
}
