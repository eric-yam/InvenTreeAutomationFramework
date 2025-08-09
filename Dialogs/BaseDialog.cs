using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Dialogs;

public abstract class BaseDialog
{
    protected IPage page;
    public BaseDialog(IPage page)
    {
        this.page = page;
    }
}