using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages;

public abstract class BaseDetailPage : BaseSectionTab
{
    private ILocator ProductHeader() => this.page.Locator("div[class*='mantine-Group-root'] div[class*='mantine-Stack-root'] p[class*='mantine-Text-root'][data-variant='gradient']");
    private ILocator ProductSubheading() => this.page.Locator("div[class*='mantine-Group-root'] div[class*='mantine-Stack-root'] p[class*='mantine-Text-root'][data-size='sm']");
    private ILocator BarcodeDropdownButton() => this.page.Locator("button[aria-label*='action-menu-barcode-actions']");
    private ILocator PrintOptionsDropdown() => this.page.Locator("button[aria-label*='action-menu-printing-actions']");
    private ILocator ItemAdditionalActions() => this.page.Locator("button[aria-label*='action-menu'][data-variant='transparent']");
    private ILocator ButtonDropdown() => this.page.Locator("div[class*='mantine-Menu-dropdown'] button div[class*='itemLabel']");

    public BaseDetailPage(IPage page) : base(page) { }

    public async Task<string> GetProductHeader() { return await this.ProductHeader().TextContentAsync() ?? ""; }
    public async Task<string> GetProductSubheading() { return await this.ProductSubheading().TextContentAsync() ?? ""; }
    public async Task ClickBarcodeDropdownButton() { await this.BarcodeDropdownButton().ClickAsync(); }
    public async Task ClickPrintOptionsDropdown() { await this.PrintOptionsDropdown().ClickAsync(); }
    public async Task ClickItemAdditionalActions() { await this.ItemAdditionalActions().ClickAsync(); }

    public async Task SelectDropdown(string option)
    {
        await this.ButtonDropdown().WaitForAsync();
        var dropdownList = await this.ButtonDropdown().AllAsync();

        foreach (var item in dropdownList)
        {
            string? s = await item.TextContentAsync();
            if (s != null && s.Equals(option))
            {
                await item.ClickAsync();
                break;
            }
        }
    }
}