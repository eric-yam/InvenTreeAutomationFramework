using System.Threading.Tasks;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Components;

public class HomeTabStrip : BaseComponent
{
    private const string BUTTON = "button[class*='mantine-Tabs-tab']";
    private ILocator TabStripButtons() => this.page.Locator("div[role='tablist'][data-orientation='horizontal']");
    Dictionary<string, ILocator> TabStripButtonsDictionary;
    public HomeTabStrip(IPage page) : base(page) { this.TabStripButtonsDictionary = new Dictionary<string, ILocator>(); }

    public async Task SetTabstrip()
    {
        await this.TabStripButtons().WaitForAsync();
        var tabList = await this.TabStripButtons().Locator(BUTTON).AllAsync();

        //Only need to initialize once
        if (this.TabStripButtonsDictionary.Count == 0)
        {
            foreach (var tab in tabList)
            {
                string? tabName = await tab.TextContentAsync();
                if (tabName != null)
                {
                    this.TabStripButtonsDictionary.Add(tabName, tab);
                }
            }
        }
    }

    public Dictionary<string, ILocator> GetTabStripButtonlist()
    {
        return this.TabStripButtonsDictionary;
    }
}