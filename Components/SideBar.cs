using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components;

public class SideBar : BaseComponent
{
    private const string SIDE_BAR_BUTTONS = "button[class*='Tab']";
    private ILocator SideBarButtons() => this.page.Locator("div[class*='mantine-Paper-root'] div[class*='mantine-Tabs-list']");

    private Dictionary<string, ILocator> SideBarButtonsDictionary;
    public SideBar(IPage page) : base(page) { this.SideBarButtonsDictionary = new Dictionary<string, ILocator>(); }

    public async Task SetSideBar()
    {
        await this.SideBarButtons().WaitForAsync();
        var sideBarList = await this.SideBarButtons().Locator(SIDE_BAR_BUTTONS).AllAsync();

        // if (this.SideBarButtonsDictionary.Count > 0)

        this.SideBarButtonsDictionary.Clear();

        foreach (var tab in sideBarList)
        {
            string? tabName = await tab.TextContentAsync();
            if (tabName != null)
            {
                this.SideBarButtonsDictionary.Add(tabName, tab);
            }
        }
    }

    public Dictionary<string, ILocator> GetSideBarButtonlist()
    {
        return this.SideBarButtonsDictionary;
    }
}