using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Util;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Forms.Manufacturing;

public class BuildOrderForm : BaseForm
{
    private ILocator BuildOrderRefInput() => this.page.Locator("input[help_text='Build Order Reference']");
    private ILocator PartDropdownButton() => this.page.Locator("div[help_text='Select part to build'] div[class=' css-23xn0o-indicatorContainer']");
    private ILocator DescriptionInput() => this.page.Locator("input[help_text='Brief description of the build (optional)']");
    private ILocator BuildQuantityInput() => this.page.Locator("input[name='quantity']");
    private ILocator IncrementBuildQuantityButton() => this.page.Locator("input[name='quantity'] + div button[class*='mantine-NumberInput-control'][data-direction='up']");
    private ILocator DecrementBuildQuantityButton() => this.page.Locator("input[name='quantity'] + div button[class*='mantine-NumberInput-control'][data-direction='down']");
    private ILocator TargetCompletionDateInput() => this.page.Locator("input[aria-label='date-field-target_date']");
    private ILocator ExternalLinkInput() => this.page.Locator("input[help_text='Link to external URL']");
    private ILocator ResponsibleDropdownButton() => this.page.Locator("div[name='responsible'] div[class=' css-23xn0o-indicatorContainer']");

    public BuildOrderForm(IPage page) : base(page) { }

    //Actions
    [AllureStep("User Fills Build Order Form")]
    public async Task FillForm(string part, string desc, string quantity, string targetDate, string external, string responsible)
    {
        await this.SelectPart(part);
        await this.FillDescription(desc);
        await this.FillBuildQuantity(quantity);
        await this.FillTargetCompletionDate(targetDate);
        await this.FillExternalLink(external);
        await this.SelectResponsible(responsible);
    }

    [AllureStep("User Selects Part To Build [{option}]")]
    public async Task SelectPart(string option)
    {
        await this.PartDropdownButton().ClickAsync();
        await DropDownHelper.SelectListboxOption(this.page, option);
    }

    [AllureStep("User Selects User Responsible For Build Order [{option}]")]
    public async Task SelectResponsible(string option)
    {
        await this.ResponsibleDropdownButton().ClickAsync();
        await DropDownHelper.SelectListboxOption(this.page, option);
    }

    [AllureStep("User Inputs Build Order Reference [{input}]")]
    public async Task FillBuildOrderRef(string input) { await this.BuildOrderRefInput().FillAsync(input); }

    [AllureStep("User Inputs Description [{input}]")]
    public async Task FillDescription(string input) { await this.DescriptionInput().FillAsync(input); }

    [AllureStep("User Inputs Build Quantity [{input}]")]
    public async Task FillBuildQuantity(string input) { await this.BuildQuantityInput().FillAsync(input); }

    [AllureStep("User Inputs Target Completion Date [{input}]")]
    public async Task FillTargetCompletionDate(string input) { await this.TargetCompletionDateInput().FillAsync(input); }

    [AllureStep("User Inputs External Link [{input}]")]
    public async Task FillExternalLink(string input) { await this.ExternalLinkInput().FillAsync(input); }

    [AllureStep("User Clicks Increment Build Quantity Button")]
    public async Task ClickIncrementBuildQuantityButton() { await this.IncrementBuildQuantityButton().ClickAsync(); }

    [AllureStep("User Clicks Decrement Build Quantity Button")]
    public async Task ClickDecrementBuildQuantityButton() { await this.DecrementBuildQuantityButton().ClickAsync(); }

}