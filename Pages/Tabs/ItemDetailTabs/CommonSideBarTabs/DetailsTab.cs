using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.Tabs.ItemDetailTabs.CommonSideBarTabs;

public class DetailsTab : BaseDetailTab
{
    private ILocator DetailTables() => this.page.Locator("table tr");

    private Dictionary<string, string> ProductInformation;

    public DetailsTab(IPage page) : base(page)
    {
        this.ProductInformation = new Dictionary<string, string>();
    }

    public async Task InitializeDetails()
    {
        var infoTable = await this.DetailTables().AllAsync();

        foreach (var row in infoTable)
        {
            //TODO: Refactor as constants
            string? key = await row.Locator("td:nth-child(1)").TextContentAsync();
            string? val = await row.Locator("td:nth-child(2)").TextContentAsync();

            if (string.IsNullOrEmpty(val))
            {
                val = await row.Locator("td:nth-child(2) p").TextContentAsync();
            }

            if (key != null && val != null)
            {
                this.ProductInformation.Add(key, val);
            }
        }
    }

    public Dictionary<string, string> GetDetails()
    {
        return this.ProductInformation;
    }
}