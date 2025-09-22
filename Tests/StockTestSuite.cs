using Allure.NUnit;
using InvenTreeAutomationFramework.Enums;
using InvenTreeAutomationFramework.Pages.Home;
using InvenTreeAutomationFramework.Pages.Login;
using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.StockSection;
using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.StockSection.SideBarTabs;
using InvenTreeAutomationFramework.Util;

namespace InvenTreeAutomationFramework.Tests;

[AllureNUnit]
public class StockTestSuite : BaseTest
{
    [Test]
    public async Task Test_1_Add_Stock()
    {
        //Login
        LoginPage loginPage = new LoginPage(Page);
        await loginPage.UserLogin(username, password);
        await APIHelper.StartWaitingForResponse(Page, APIEndpoints.APIEndpointDictionary[APIHelperEnums.UserMe]);

        //Navigate to Stocks tab
        HomePage hp = new HomePage(Page);
        await hp.SelectTab(Navigation.NavigationSteps[NavigationEnums.Stock]);

        StockSectionTab sst = hp.GetStockSectionTab();
        StockItemsTab sit = sst.GetStockItemsTab();
        await sst.SelectSideBarTab("Stock Items");
        await sit.InitializeTable();
    }
}

//When landing on section tab, need to click the tab
//when navigatin on home page tabstrip, simplify purpose of homepage
//Maybe add to homepage strip to simply return an instance of section tabs 