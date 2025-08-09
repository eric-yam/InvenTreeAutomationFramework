using InvenTreeAutomationFramework.Components;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.SectionTabs;

public abstract class BaseInvenTablePage : BasePage
{
    private ILocator AddButton() => this.page.Locator("button[aria-label='action-button-add-build-order']");
    protected Table table;

    public BaseInvenTablePage(IPage page) : base(page)
    {
        this.table = new Table(this.page);
    }

    public Table GetTableObj()
    {
        return this.table;
    }

    //Actions
    public async Task ClickAddButton()
    {
        await this.AddButton().ClickAsync();
    }
}
