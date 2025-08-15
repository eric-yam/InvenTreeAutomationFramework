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
    private ILocator DropdownOptions() => this.page.Locator("div[class*='mantine-Menu-dropdown'] button div[class*='itemLabel']");


    public BaseDetailTab(IPage page) : base(page) { }

    public async Task<string> GetProductHeader() { return await this.ProductHeader().TextContentAsync() ?? ""; }
    public async Task<string> GetProductSubheading() { return await this.ProductSubheading().TextContentAsync() ?? ""; }
    public async Task ClickBarcodeDropdownButton() { await this.BarcodeDropdownButton().ClickAsync(); }
    public async Task ClickPrintOptionsDropdown() { await this.PrintOptionsDropdownButton().ClickAsync(); }
    public async Task ClickThreeDots() { await this.ThreeDotsDropdownButton().ClickAsync(); }

    public async Task SelectDropdown(string option)
    {
        await DropDownHelper.SelectMantineMenuOption(this.page, option);
    }
}