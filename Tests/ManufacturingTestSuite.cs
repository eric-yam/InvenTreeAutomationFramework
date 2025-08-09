using Allure.NUnit;
using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Components;
using InvenTreeAutomationFramework.Dialogs;
using InvenTreeAutomationFramework.Enums;
using InvenTreeAutomationFramework.Forms.Manufacturing;
using InvenTreeAutomationFramework.Pages.Home;
using InvenTreeAutomationFramework.Pages.SectionTabs;
using InvenTreeAutomationFramework.Pages.Login;
using InvenTreeAutomationFramework.Util;
using InvenTreeAutomationFramework.Pages.SideBarTabs.Manufacturing;

namespace InvenTreeAutomationFramework.Tests;

[AllureNUnit]
[AllureEpic("InvenTree Demo Mode")]
[AllureFeature("Manufacturing")]
public class ManufacturingTestSuite : BaseTest
{
    //TODO: Factor out the test data into external Test Data files
    [Test]
    [AllureOwner("Eric Yam ")]
    [AllureName("Manufacturing - Add Build Order")]
    [AllureTag("Manufacturing")]
    [Description("User logs in and adds a new build order in the manufacturing section. Table is validated to ensure new build order is added in the manufacturing inventory table.")]
    public async Task Test_1_Add_Build_Form()
    {
        //Login
        LoginPage loginPage = new LoginPage(Page);
        SetUserRole(UserRoles.RolesDict[UserEnums.Admin]);
        await loginPage.UserLogin(username, password);
        await APIHelper.StartWaitingForResponse(Page, APIEndpoints.APIEndpointDictionary[APIHelperEnums.UserMe]);

        //Verify Logged In Success
        NotificationDialog notificationDialog = new NotificationDialog(Page);
        Assert.That(await notificationDialog.VerifyNotifMsg(NotificationEnums.LoginSuccess), Is.EqualTo(true), "Notification for successful login did not appear. User failed to login");

        //Navigate to Manufacturing Page
        HomePage homePage = new HomePage(Page);
        await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Manufacturing]);

        ManufacturingSectionTab mst = homePage.GetManufacturingTab();
        BuildOrderTab buildOrderTab = mst.GetBuildOrderTab();
        // ManufacturingTab mt = homePage.GetManufacturingTab();
        await buildOrderTab.InitializeTable();
        var response = APIHelper.GetResponse();

        //Open/Fill Build Add Form 
        await buildOrderTab.ClickAddButton();
        AddBuildForm addBuildForm = new AddBuildForm(Page);
        await addBuildForm.FillForm("Blue Chair", "Automation - Test", "2", "2025-12-04", "", "admin");
        await addBuildForm.ClickSubmitButton();

        //Verify Successfully Created New Item Notification Visible
        Assert.That(await notificationDialog.VerifyNotifMsg(NotificationEnums.SuccessItemCreated), Is.EqualTo(true), "Notification for successful item created did not appear. Failed to create new item");

        //Verify Added To Table
        await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Manufacturing]);
        await buildOrderTab.InitializeTable();
        response = APIHelper.GetResponse();

        Table mtTable = buildOrderTab.GetTableObj();
        Dictionary<string, Dictionary<string, string>> updatedResults = APIHelper.GetResponseTableResults(response, await mtTable.GetHeaderNames());
        // List<JsonElement> updatedResults = APIHelper.GetListProperty(APIPropertyEnums.Results);
        // updatedResults.First();

        Dictionary<string, string> updatedResultsValue = updatedResults.First().Value;
        TableRow newRow = mtTable.GetRow(updatedResultsValue["Reference"]);
        Assert.That(newRow.GetRowAsDictionary().Except(updatedResultsValue).Any(), Is.EqualTo(false),
                $"UI: {AssertionMessageHelper.PrintEnumerable(newRow.GetRowAsDictionary())} " +
                $"Backend: {AssertionMessageHelper.PrintEnumerable(updatedResultsValue)} " +
                $"Difference: {AssertionMessageHelper.PrintEnumerable(newRow.GetRowAsDictionary().Except(updatedResultsValue))}");
    }

    //Edit Manufcaturing Item 
}