using Allure.NUnit;
using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Components;
using InvenTreeAutomationFramework.Dialogs;
using InvenTreeAutomationFramework.Enums;
using InvenTreeAutomationFramework.Pages.Home;
using InvenTreeAutomationFramework.Pages.SectionTabs;
using InvenTreeAutomationFramework.Pages.Login;
using InvenTreeAutomationFramework.Util;
using InvenTreeAutomationFramework.Pages.SideBarTabs.Manufacturing;

namespace InvenTreeAutomationFramework.Tests;

[AllureNUnit]
[AllureEpic("Scrap Test Epic")]
[AllureFeature("Scrap Test Feature")]
public class Tester : BaseTest
{
    [Test]
    [AllureOwner("Eric Yam ")]
    [AllureName("Temporary Test")]
    [AllureTag("Test_Tag")]
    [Description("Temporary test to check code compiles.")]
    public async Task Test1()
    {
        LoginPage loginPage = new LoginPage(Page);
        SetUserRole(UserRoles.RolesDict[UserEnums.Admin]);
        await loginPage.UserLogin(username, password);
        await APIHelper.StartWaitingForResponse(Page, APIEndpoints.APIEndpointDictionary[APIHelperEnums.UserMe]);

        NotificationDialog notificationDialog = new NotificationDialog(Page);
        Assert.That(await notificationDialog.VerifyNotifMsg(NotificationEnums.LoginSuccess), Is.EqualTo(true), "Notification for successful login did not appear. User failed to login");

        var temp = APIHelper.GetResponse();

        HomePage homePage = new HomePage(Page);
        // await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Parts]);
        // await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Stock]);
        await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Manufacturing]);
        // await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Purchasing]);
        // await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Sales]);

        // UserDataModel dm2 = TestDataProvider.DeserializeResponse<UserDataModel>(temp.ToString());

        ManufacturingSectionTab mst = new ManufacturingSectionTab(Page);
        BuildOrderTab buildOrderTab = mst.GetBuildOrderTab();
        // ManufacturingTab mt = homePage.GetManufacturingTab();
        await buildOrderTab.InitializeTable();
        temp = APIHelper.GetResponse(); //Get Response for manufacturing table 

        await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Manufacturing]);
        ManufacturingSectionTab mst2 = new ManufacturingSectionTab(Page);
        BuildOrderTab buildOrderTab2 = mst.GetBuildOrderTab();
        // ManufacturingTab mp2 = new ManufacturingTab(Page);
        await buildOrderTab2.InitializeTable();

        Assert.That(buildOrderTab.GetTableObj().Equals(buildOrderTab2.GetTableObj()), Is.EqualTo(true));

        Dictionary<string, Dictionary<string, string>> ResponseResults = APIHelper.GetResponseTableResults(temp, await buildOrderTab.GetTableObj().GetHeaderNames());

        // bool isEqual = ResponseResults.Values.First().Except(mp.GetTable().GetRowAsDictionary("BO0027")).Any();    

        foreach (Dictionary<string, string> respVal in ResponseResults.Values)
        {
            TableRow row = buildOrderTab.GetTableObj().GetRow(respVal["Reference"]); // need to get different key for different tables
            Assert.That(row.GetRowAsDictionary().Except(respVal).Any(), Is.EqualTo(false),
                $"UI: {AssertionMessageHelper.PrintEnumerable(row.GetRowAsDictionary())} " +
                $"Backend: {AssertionMessageHelper.PrintEnumerable(respVal)} " +
                $"Difference: {AssertionMessageHelper.PrintEnumerable(row.GetRowAsDictionary().Except(respVal))}");
        }
    }
}