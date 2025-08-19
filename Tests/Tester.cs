using Allure.NUnit;
using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Components;
using InvenTreeAutomationFramework.Dialogs;
using InvenTreeAutomationFramework.Enums;
using InvenTreeAutomationFramework.Pages.Home;
using InvenTreeAutomationFramework.Pages.Login;
using InvenTreeAutomationFramework.Util;
using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.ManufacturingSection;
using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.ManufacturingSection.SideBarTabs;
using InvenTreeAutomationFramework.Pages.Tabs.ItemDetailTabs.CommonSideBarTabs;
using InvenTreeAutomationFramework.Pages.ItemDetailTabs;
using System.Text.Json;
using InvenTreeAutomationFramework.Forms.Manufacturing;

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
        await loginPage.UserLogin(username, password, language);
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

    [Test]
    public async Task Test2()
    {
        //Login
        LoginPage loginPage = new LoginPage(Page);
        SetUserRole(UserRoles.RolesDict[UserEnums.Admin]);
        await loginPage.UserLogin(username, password, language);
        await APIHelper.StartWaitingForResponse(Page, APIEndpoints.APIEndpointDictionary[APIHelperEnums.UserMe]);

        //Verify Logged In Success
        NotificationDialog notificationDialog = new NotificationDialog(Page);
        Assert.That(await notificationDialog.VerifyNotifMsg(NotificationEnums.LoginSuccess), Is.EqualTo(true), "Notification for successful login did not appear. User failed to login");

        //Navigate to Manufacturing Page
        HomePage homePage = new HomePage(Page);
        await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Manufacturing]);

        //Landed on Manufacturing Section Page
        ManufacturingSectionTab mst = homePage.GetManufacturingTab();
        BuildOrderTab buildOrderTab = mst.GetBuildOrderTab();
        await buildOrderTab.InitializeTable();
        JsonElement? response = APIHelper.GetResponse();

        Table buildOrderTable = buildOrderTab.GetTableObj(); //Init table from UI
        List<string> headerNames = await buildOrderTable.GetHeaderNames();
        await buildOrderTable.GetRow("BO0026").ClickRow(); //Open product page

        await APIHelper.WaitForOrderDetailsResponse(Page);
        response = APIHelper.GetResponse();

        //Landed on Manufacturing Product Page        
        ManufacturingItemDetailTab midt = new ManufacturingItemDetailTab(Page);
        await midt.SelectSideBarTab("Build Details"); //TODO: Sidebar enums class 

        DetailsTab dt = midt.GetDetailsTab();
        await dt.InitializeDetails();

        Dictionary<string, string> generalPartInfo = APIHelper.TranslateHeaderToAPIKey(response, dt.GetDetails().Keys.ToList());
        //parameterize and separate out dictionaries into separate dictionaries in api header helper

        Assert.That(generalPartInfo["Part"], Is.EqualTo(dt.GetDetails()["Part"]));

        await dt.SelectThreeDotsOptions("Edit");
        BuildOrderForm abf = new BuildOrderForm(Page);

        await abf.FillDescription("Edited Description - 3");
        await abf.FillBuildQuantity("1");
        await abf.ClickSubmitButton();

        await APIHelper.WaitForOrderDetailsResponse(Page);
        response = APIHelper.GetResponse();
        await dt.InitializeDetails();

        Assert.That(await notificationDialog.VerifyNotifMsg(NotificationEnums.SuccessItemUpdated), Is.EqualTo(true), "Notification for successful item update did not appear. Failed to update item");
        
        Assert.That(response?.GetProperty("title").ToString(), Is.EqualTo(dt.GetDetails()["Description"]));



        //Validate Details and Information Displayed in table matches

        // JsonElement partDetails = APIHelper.GetPropertyInList(APIPropertyEnums.Results, APIPropertyEnums.Reference, APIPropertyEnums.PartDetail, "BO0026");

        // string s = partDetails.GetProperty("name").ToString();

        // Assert.That(s.Equals(dt.GetDetails()["Part"]), Is.EqualTo(true), $"Backend: {s} UI: {dt.GetDetails()["Part"]}");

        // // string productReference = responseResultsValue["Reference"];

        // // var temp = response?.GetProperty("part_detail");

        // await dt.SelectThreeDotsOptions("Edit");
        // AddBuildForm abf = new AddBuildForm(Page);

        // await abf.FillDescription("Edited Description");
        // await abf.FillBuildQuantity("4");
        // await abf.ClickSubmitButton();
        // Assert.That(await notificationDialog.VerifyNotifMsg(NotificationEnums.SuccessItemUpdated), Is.EqualTo(true), "Notification for successful item update did not appear. Failed to update item");

        // //Go back to homepage to get updated response
        // await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Manufacturing]);
        // partDetails = APIHelper.GetPropertyInList(APIPropertyEnums.Results, APIPropertyEnums.Reference, APIPropertyEnums.PartDetail, "BO0026");

        // s = partDetails.GetProperty("name").ToString();

        // Assert.That(s.Equals(dt.GetDetails()["Part"]), Is.EqualTo(true), $"Backend: {s} UI: {dt.GetDetails()["Part"]}");


    }
}