using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Components;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.SectionTabs;

public abstract class BaseInvenTableTab : BasePage
{
    private ILocator AddButton() => this.page.Locator("button[aria-label*='add']");
    protected Table table;

    public BaseInvenTableTab(IPage page) : base(page)
    {
        this.table = new Table(this.page);
    }

    public Table GetTableObj()
    {
        return this.table;
    }

    //Actions
    [AllureStep("User Clicks Add Button On Inventory Table")]
    public async Task ClickAddButton()
    {
        await this.AddButton().ClickAsync();
    }
}
