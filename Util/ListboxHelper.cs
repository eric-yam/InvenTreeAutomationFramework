using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Util;

public static class ListboxHelper
{
    private const string LISTBOX_LOCATOR = "div[role='listbox']";
    private const string LISTBOX_OPTION_LOCATOR = "p";
    private const string LISTBOX_LOADING_LOCATOR = "//div[contains(text(), 'Loading...')]";

    public static async Task SelectListboxOption(IPage page, string option)
    {
        await page.Locator(LISTBOX_LOCATOR).WaitForAsync();
        await page.WaitForSelectorAsync(LISTBOX_LOADING_LOCATOR, new PageWaitForSelectorOptions { State = WaitForSelectorState.Detached, Timeout = 3000 });
        var partList = await page.Locator(LISTBOX_LOCATOR).Locator(LISTBOX_OPTION_LOCATOR).AllAsync();

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
}