using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Util;

public static class DropDownHelper
{
    private const string LISTBOX_LOCATOR = "div[role='listbox']";
    private const string LISTBOX_OPTION_LOCATOR_P = "p";
    private const string LISTBOX_OPTION_LOCATOR_SPAN = "span";
    private const string LISTBOX_LOADING_LOCATOR = "//div[contains(text(), 'Loading...')]";

    private const string MANTINE_DROPDOWN_LOCATOR = "div[class*='mantine-Menu-dropdown']";
    private const string MANTINE_DROPDOWN_OPTIONS_LOCATOR = "div[class*='mantine-Menu-dropdown'] button div[class*='itemLabel']";

    public static async Task SelectListboxOption(IPage page, string option)
    {
        await page.Locator(LISTBOX_LOCATOR).WaitForAsync();
        await page.WaitForSelectorAsync(LISTBOX_LOADING_LOCATOR, new PageWaitForSelectorOptions { State = WaitForSelectorState.Detached, Timeout = 3000 });
        IReadOnlyList<ILocator> partList = await page.Locator(LISTBOX_LOCATOR).Locator(LISTBOX_OPTION_LOCATOR_P).AllAsync();
        await SelectOption(partList, option);
    }

    public static async Task SelectMantineMenuOption(IPage page, string option)
    {
        await page.Locator(MANTINE_DROPDOWN_LOCATOR).WaitForAsync();
        IReadOnlyList<ILocator> threeDotsDropdownList = await page.Locator(MANTINE_DROPDOWN_OPTIONS_LOCATOR).AllAsync();
        await SelectOption(threeDotsDropdownList, option);
    }

    private static async Task SelectOption(IReadOnlyList<ILocator> dropdownList, string option)
    {
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