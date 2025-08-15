using InvenTreeAutomationFramework.Util;
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

    public AddBuildForm(IPage page) : base(page) { }

    public async Task FillForm(string part, string desc, string quantity, string targetDate, string external, string responsible)
    {        
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
        await DropDownHelper.SelectListboxOption(this.page, option);
    }

    public async Task SelectResponsible(string option)
    {
        await this.ResponsibleDropdownButton().ClickAsync();
        await DropDownHelper.SelectListboxOption(this.page, option);
    }

    public async Task FillBuildOrderRef(string input) { await this.BuildOrderRefInput().FillAsync(input); }
    public async Task FillDescription(string input) { await this.DescriptionInput().FillAsync(input); }
    public async Task FillBuildQuantity(string input) { await this.BuildQuantityInput().FillAsync(input); }
    public async Task FillTargetCompletionDate(string input) { await this.TargetCompletionDateInput().FillAsync(input); }
    public async Task FillExternalLink(string input) { await this.ExternalLinkInput().FillAsync(input); }
}