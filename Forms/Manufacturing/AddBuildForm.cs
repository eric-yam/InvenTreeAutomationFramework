using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Forms.Manufacturing;

public class AddBuildForm : BaseForm
{
    private ILocator BuildOrderRefInput() => this.page.Locator("input[help_text='Build Order Reference']");
    private ILocator PartDropdownButton() => this.page.Locator("div[help_text='Select part to build'] div[class=' css-23xn0o-indicatorContainer']");
    private ILocator DescriptionInput() => this.page.Locator("input[help_text='Brief description of the build (optional)']");
    private ILocator BuildQuantityInput() => this.page.Locator("input[name='quantity']");
    private ILocator TargetCompletionDateInput() => this.page.Locator("input[aria-label='date-field-target_date']");
    private ILocator ExternalLinkInput() => this.page.Locator("input[help_text='Link to external URL']");
    private ILocator ResponsibleDropdownButton() => this.page.Locator("div[name='responsible'] div[class=' css-23xn0o-indicatorContainer']");

    private ILocator ListBox() => this.page.Locator("div[role='listbox']"); //TODO: To be factored out

    public AddBuildForm(IPage page) : base(page) { }

    public async Task FillForm(string part, string desc, string quantity, string targetDate, string external, string responsible)
    {
        // await this.FillBuildOrderRef(buildRef);
        await this.SelectPart(part);
        await this.FillDescription(desc);
        await this.FillBuildQuantity(quantity);
        await this.FillTargetCompletionDate(targetDate);
        await this.FillExternalLink(external);
        await this.SelectResponsible(responsible);
    }

    public async Task SelectPart(string option)
    {
        await this.PartDropdownButton().ClickAsync();
        await this.ListBox().WaitForAsync();
        await this.page.WaitForSelectorAsync("//div[contains(text(), 'Loading...')]", new PageWaitForSelectorOptions { State = WaitForSelectorState.Detached, Timeout = 3000 });
        var partList = await this.ListBox().Locator("p").AllAsync();

        foreach (var item in partList)
        {
            string? s = await item.TextContentAsync();
            if (s != null && s.Equals(option))
            {
                await item.ClickAsync();
                break;
            }
        }
    }

    public async Task SelectResponsible(string option)
    {
        await this.ResponsibleDropdownButton().ClickAsync();
        await this.ListBox().WaitForAsync();
        await this.page.WaitForSelectorAsync("//div[contains(text(), 'Loading...')]", new PageWaitForSelectorOptions { State = WaitForSelectorState.Detached, Timeout = 3000 });
        var responsibleList = await this.ListBox().Locator("p").AllAsync();

        foreach (var item in responsibleList)
        {
            string? s = await item.TextContentAsync();
            if (s != null && s.Equals(option))
            {
                await item.ClickAsync();
                break;
            }
        }
    }

    public async Task FillBuildOrderRef(string input) { await this.BuildOrderRefInput().FillAsync(input); }
    public async Task FillDescription(string input) { await this.DescriptionInput().FillAsync(input); }
    public async Task FillBuildQuantity(string input) { await this.BuildQuantityInput().FillAsync(input); }
    public async Task FillTargetCompletionDate(string input) { await this.TargetCompletionDateInput().FillAsync(input); }
    public async Task FillExternalLink(string input) { await this.ExternalLinkInput().FillAsync(input); }
}