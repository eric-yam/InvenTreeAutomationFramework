using InvenTreeAutomationFramework.Pages.Tabs.ItemDetailTabs.CommonSideBarTabs;
using Microsoft.Playwright;

namespace InvenTreeAutomationFramework.Pages.ItemDetailTabs;

public class ManufacturingItemDetailTab : BaseSectionTab
{
    private DetailsTab buildDetails;
    //TODO: Implement other sidebar tabs
    //Required Parts
    //Allocated Stock
    //...
    //Notes

    public ManufacturingItemDetailTab(IPage page) : base(page)
    {
        this.buildDetails = new DetailsTab(this.page);
    }

    public DetailsTab GetDetailsTab()
    {
        return this.buildDetails;
    }
}