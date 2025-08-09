using InvenTreeAutomationFramework.Components;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.SectionTabs;

public class ManufacturingTab : AppPage
{
    private ILocator AddButton() => this.page.Locator("button[aria-label='action-button-add-build-order']"); //TODO: to be factored out (Create subclass specifying inventory pages)
    private Table ManufacturingPageTable;
    public ManufacturingTab(IPage page) : base(page)
    {
        this.ManufacturingPageTable = new Table(this.page);
    }

    public async Task InitializeTable()
    {
        await this.ManufacturingPageTable.InitializeTable(TableRowFactory.InitManufacturingTableRow);
    }

    public Table GetTableObj()
    {
        return this.ManufacturingPageTable;
    }

    //Actions
    public async Task ClickAddButton()
    {
        await this.AddButton().ClickAsync();
    }

}