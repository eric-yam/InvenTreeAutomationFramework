using InvenTreeAutomationFramework.Util;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages;

public abstract class BaseDetailTab : BasePage
{
    private ILocator ProductHeader() => this.page.Locator("div[class*='mantine-Group-root'] div[class*='mantine-Stack-root'] p[class*='mantine-Text-root'][data-variant='gradient']");
    private ILocator ProductSubheading() => this.page.Locator("div[class*='mantine-Group-root'] div[class*='mantine-Stack-root'] p[class*='mantine-Text-root'][data-size='sm']");
    private ILocator BarcodeDropdownButton() => this.page.Locator("button[aria-label*='action-menu-barcode-actions']");
    private ILocator PrintOptionsDropdownButton() => this.page.Locator("button[aria-label*='action-menu-printing-actions']");
    private ILocator ThreeDotsDropdownButton() => this.page.Locator("button[aria-label*='action-menu'][data-variant='transparent']");

    public BaseDetailTab(IPage page) : base(page) { }

    public async Task<string> GetProductHeader() { return await this.ProductHeader().TextContentAsync() ?? ""; }
    public async Task<string> GetProductSubheading() { return await this.ProductSubheading().TextContentAsync() ?? ""; }
    public async Task SelectBarcodeDropdownOption(string option)
    {
        await this.BarcodeDropdownButton().ClickAsync();
        await DropDownHelper.SelectMantineMenuOption(this.page, option);
    }
    public async Task SelectPrintOptions(string option)
    {
        await this.PrintOptionsDropdownButton().ClickAsync();
        await DropDownHelper.SelectMantineMenuOption(this.page, option);
    }
    public async Task SelectThreeDotsOptions(string option)
    {
        await this.ThreeDotsDropdownButton().ClickAsync();
        await DropDownHelper.SelectMantineMenuOption(this.page, option);
    }
}