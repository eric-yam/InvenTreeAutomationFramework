using Allure.NUnit;
using Allure.NUnit.Attributes;
using InvenTreeAutomationFramework.Components;
using InvenTreeAutomationFramework.Dialogs;
using InvenTreeAutomationFramework.Enums;
using InvenTreeAutomationFramework.Forms.Manufacturing;
using InvenTreeAutomationFramework.Pages.Home;
using InvenTreeAutomationFramework.Pages.Login;
using InvenTreeAutomationFramework.Util;
using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.ManufacturingSection;
using InvenTreeAutomationFramework.Pages.Tabs.SectionTabs.ManufacturingSection.SideBarTabs;
using InvenTreeAutomationFramework.Pages.ItemDetailTabs;
using InvenTreeAutomationFramework.Pages.Tabs.ItemDetailTabs.CommonSideBarTabs;
using System.Text.Json;

namespace InvenTreeAutomationFramework.Tests;

[AllureNUnit]
[AllureEpic("InvenTree Demo Mode")]
[AllureFeature("Manufacturing")]
public class ManufacturingTestSuite : BaseTest
{
    /*
        TODO: Factor out the test data into external Test Data files
        TODO: Sometimes the account doesn't start in the english language. In that case, we should use an API call before the automation starts to 
        set the language to english     
            Request URL https://demo.inventree.org/api/user/profile/
            Request Method PATCH
    */
    [Test]
    [AllureOwner("Eric Yam")]
    [AllureName("Manufacturing - Add Build Order")]
    [AllureTag("Manufacturing")]
    [Description("User logs in and adds a new build order in the manufacturing section. Table is validated to ensure new build order is added in the manufacturing inventory table.")]
    public async Task Test_1_Add_Build_Form()
    {

        /*
            As of 2025/08/17, they added an additional tag in the Issued By column that also indicates Active or Inactive for the
            Issued By person, but it is not present in the API response. Currently unsure where to find the information
            in the responses for the Inactive tag. This is causing the final validation to fail as the UI displays the 
            additional information of Inactive, whereas the API cannot.

            Currently investigating..

            Currently the website has been reverted and no longer includes the tag
        */
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
        var temp = newRow.Contains(updatedResultsValue);

        Assert.That(temp, Is.EqualTo(true),
                $"UI: {AssertionMessageHelper.PrintEnumerable(newRow.GetRowAsDictionary())} " +
                $"Backend: {AssertionMessageHelper.PrintEnumerable(updatedResultsValue)} " +
                $"Difference: {AssertionMessageHelper.PrintEnumerable(newRow.GetRowAsDictionary().Except(updatedResultsValue))}");

        // Assert.That(newRow.GetRowAsDictionary().Except(updatedResultsValue).Any(), Is.EqualTo(false),
        //         $"UI: {AssertionMessageHelper.PrintEnumerable(newRow.GetRowAsDictionary())} " +
        //         $"Backend: {AssertionMessageHelper.PrintEnumerable(updatedResultsValue)} " +
        //         $"Difference: {AssertionMessageHelper.PrintEnumerable(newRow.GetRowAsDictionary().Except(updatedResultsValue))}");
    }

    //Edit Manufcaturing Item 
    [Test]
    [AllureOwner("Eric Yam")]
    [AllureName("Manufacturing - Edit Build Order")]
    [AllureTag("Manufacturing")]
    [Description("User logs in and edits an existing build order in the manufacturing section. Table is validated to ensure edits made to build order is visible in the UI/API")]
    public async Task Test_2_Edit_Build_Form()
    {
        //Login
        LoginPage loginPage = new LoginPage(Page);
        SetUserRole(UserRoles.RolesDict[UserEnums.Admin]);
        await loginPage.UserLogin(username, password);
        await APIHelper.StartWaitingForResponse(Page, APIEndpoints.APIEndpointDictionary[APIHelperEnums.UserMe]);

        //Verify Logged In Success
        NotificationDialog notificationDialog = new NotificationDialog(Page);
        Assert.That(await notificationDialog.VerifyNotifMsg(NotificationEnums.LoginSuccess), Is.EqualTo(true), "Notification for successful login did not appear. User failed to login");

        //Navigate to Manufacturing Section Page
        HomePage homePage = new HomePage(Page);
        await homePage.SelectTab(Navigation.NavigationSteps[NavigationEnums.Manufacturing]);

        ManufacturingSectionTab mst = homePage.GetManufacturingTab();
        BuildOrderTab buildOrderTab = mst.GetBuildOrderTab();
        await buildOrderTab.InitializeTable();
        JsonElement? response = APIHelper.GetResponse();

        Table buildOrderTable = buildOrderTab.GetTableObj();
        await buildOrderTable.GetRow("BO0026").ClickRow();

        await APIHelper.WaitForOrderDetailsResponse(Page); //Response set to details page
        response = APIHelper.GetResponse();

        //Landed on Manufacturing Product Details Section
        ManufacturingItemDetailTab midt = new ManufacturingItemDetailTab(Page);
        DetailsTab dt = midt.GetDetailsTab();
        await dt.InitializeDetails(); //initialized page details

        // string initialPartDescription = ;
        Assert.That(dt.GetDetails()["Description"], Is.EqualTo(response?.GetProperty("title").ToString()),
            $"UI: [{dt.GetDetails()["Description"]}] Backend: [{response?.GetProperty("title").ToString()}]");

        //Open Build Order Form - Perform Edits 
        await dt.SelectThreeDotsOptions("Edit");
        AddBuildForm abf = new AddBuildForm(Page);
        await abf.FillDescription("External PCB assembly - Edited Description - " + DateTime.Now);
        await abf.ClickIncrementBuildQuantityButton();
        await abf.ClickSubmitButton();

        //Set the update response/Set the updated page details
        await APIHelper.WaitForOrderDetailsResponse(Page); //Response set to details page Updated
        response = APIHelper.GetResponse();
        await dt.InitializeDetails(); //Initialize page details Updated

        //Verify Notification For Successful Item Info Update
        Assert.That(await notificationDialog.VerifyNotifMsg(NotificationEnums.SuccessItemUpdated), Is.EqualTo(true));

        //Verify Updated Details
        Assert.That(dt.GetDetails()["Description"], Is.EqualTo(response?.GetProperty("title").ToString()),
            $"UI: [{dt.GetDetails()["Description"]}] Backend: [{response?.GetProperty("title").ToString()}]");

        Assert.That(Convert.ToDouble(dt.GetDetails()["Build Quantity"]).ToString("F1"), Is.EqualTo(response?.GetProperty("quantity").ToString()),
            $"UI: [{Convert.ToDouble(dt.GetDetails()["Build Quantity"]).ToString("F1")}] Backend: [{response?.GetProperty("quantity").ToString()}]");

    }
}